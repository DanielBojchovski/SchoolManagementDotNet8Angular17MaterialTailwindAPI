﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Consts;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<OperationStatusResponse> Register(RegisterUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);

            if (isExistsUser is not null)
                return new OperationStatusResponse()
                {
                    IsSuccessful = false,
                    Message = "UserName Already Exists"
                };

            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new OperationStatusResponse()
                {
                    IsSuccessful = false,
                    Message = errorString
                };
            }

            // Add a Default USER Role to all users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"http://localhost:4200/account/confirm-email/{token}/{newUser.Email}";

            SendEmailRequest emailRequest = new()
            {
                EmailTo = new List<string> { request.Email },
                EmailSubject = "Confirm your school management account.",
                EmailBody = $"Hello {request.UserName}, please confirm you account by clicking on the following link <p><a href=\"{url}\">Click here</a></p>"
            };

            await _emailService.SendEmail(emailRequest);

            return new OperationStatusResponse()
            {
                IsSuccessful = true,
                Message = "User Created Successfully"
            };
        }

        public async Task<OperationStatusResponse> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) 
                return new OperationStatusResponse { IsSuccessful = false, Message = "This email address has not been registered yet" };

            if(user.EmailConfirmed == true)
                return new OperationStatusResponse { IsSuccessful = false, Message = "Your email was confirmed before. Please login to your account" };

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return new OperationStatusResponse { IsSuccessful = true, Message = "Your email address is confirmed. You can login now" };
            else
                return new OperationStatusResponse { IsSuccessful = false, Message = "Something went wrong please try again" };
        }

        public async Task<OperationStatusResponse> ResendEmailConfirmation(ResendEmailConfirmationRequest request)
        {
            if (string.IsNullOrEmpty(request.Email)) 
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid email" };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) 
                return new OperationStatusResponse { IsSuccessful = false, Message = "This email address has not been registerd yet" };

            if (user.EmailConfirmed == true)
                return new OperationStatusResponse { IsSuccessful = false, Message = "Your email address was confirmed before. Please login to your account" };

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"http://localhost:4200/account/confirm-email/{token}/{user.Email}";

            SendEmailRequest emailRequest = new()
            {
                EmailTo = new List<string> { user.Email! },
                EmailSubject = "Confirm your school management account.",
                EmailBody = $"Hello {user.UserName}, please confirm you account by clicking on the following link <p><a href=\"{url}\">Click here</a></p>"
            };

            var emailResponse = await _emailService.SendEmail(emailRequest);

            return new OperationStatusResponse { IsSuccessful = emailResponse.EmailsWhichRecieveMail.Count > 0 };
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return new LoginResponse()
                {
                    IsSuccessful = false,
                    Message = "Invalid Credentials",
                    JwtToken = null
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
                return new LoginResponse()
                {
                    IsSuccessful = false,
                    Message = "Invalid Credentials",
                    JwtToken = null
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("email", user.Email ?? "")
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("roles", userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new LoginResponse()
            {
                IsSuccessful = true,
                Message = "",
                JwtToken = token
            };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"]!));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JwtOptions:Issuer"],
                    audience: _configuration["JwtOptions:Audience"],
                    expires: DateTime.Now.AddHours(8),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}
