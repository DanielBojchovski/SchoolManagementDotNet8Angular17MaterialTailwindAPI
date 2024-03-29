﻿using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Redis.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly SchoolManagementDotNet8Angular17MaterialTailwindDBContext _context;
        private readonly ICacheService _cacheService;

        public ProfessorService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<OperationStatusResponse> CreateProfessor(CreateProfessorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                Entities.Professor professorDto = new() { Name = request.Name, SchoolId = request.SchoolId };

                _context.Add(professorDto);
                await _context.SaveChangesAsync();

                _cacheService.RemoveData("professors");

                return new OperationStatusResponse { IsSuccessful = true, Message = "Success. Professor created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<DeleteProfessorResponse> DeleteProfessor(IdRequest request)
        {
            try
            {
                var professorDto = await _context.Professor.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (professorDto is null)
                {
                    return new DeleteProfessorResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"Professor with ID {request.Id} not found."
                        },
                        GetAllProfessorsResponse = await GetAllProfessors()
                    };
                }

                _context.Remove(professorDto);

                int rowsChanged = await _context.SaveChangesAsync();

                if (rowsChanged > 0)
                {
                    _cacheService.RemoveData("professors");

                    return new DeleteProfessorResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. Professor deleted successfully."
                        },
                        GetAllProfessorsResponse = await GetAllProfessors()
                    };
                }
                else
                {
                    return new DeleteProfessorResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllProfessorsResponse = await GetAllProfessors()
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteProfessorResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllProfessorsResponse = await GetAllProfessors()
                };
            }
        }

        public async Task<GetAllProfessorsResponse> GetAllProfessors()
        {
            var cacheData = _cacheService.GetData<GetAllProfessorsResponse>("professors");

            if (cacheData is not null && cacheData.List.Count > 0)
                return cacheData;

            var list = await _context.Professor
                .AsNoTracking()
                .Select(x => new ProfessorModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            GetAllProfessorsResponse response = new() { List = list };

            _cacheService.SetData("professors", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<ProfessorModel?> GetProfessorById(IdRequest request)
        {
            var response = await _context.Professor
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new ProfessorModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<GetAllProfessorsResponse> GetProfessorsBySchoolId(IdRequest request)
        {
            var response = await _context.Professor
                .Where(x => x.SchoolId == request.Id)
                .AsNoTracking()
                .Select(x => new ProfessorModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            return new GetAllProfessorsResponse { List = response };
        }

        public async Task<OperationStatusResponse> UpdateProfessor(UpdateProfessorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                var professorDto = await _context.Professor.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (professorDto is null)
                    return new OperationStatusResponse { IsSuccessful = false, Message = $"Professor with ID {request.Id} not found." };

                professorDto.Name = request.Name;
                professorDto.SchoolId = request.SchoolId;
                await _context.SaveChangesAsync();

                _cacheService.RemoveData("professors");

                return new OperationStatusResponse { IsSuccessful = true, Message = $"Success. Professor with ID {professorDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<DropDownResponse> GetSchoolsDropDown()
        {
            var cacheData = _cacheService.GetData<DropDownResponse>("schoolsDropDownForProfessor");

            if (cacheData is not null && cacheData.List.Count > 0)
                return cacheData;

            var list = await _context.School
                .AsNoTracking()
                .Select(x => new DropDownItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            DropDownResponse response = new() { List = list };

            _cacheService.SetData("schoolsDropDownForProfessor", response, DateTimeOffset.Now.AddMonths(1));

            return response;
        }

        public async Task<InitUpdateProfessorResponse> InitUpdateProfessor(IdRequest request)
        {
             var professor = await _context.Professor
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new ProfessorDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolId = x.School.Id
                }).FirstOrDefaultAsync();

            var schoolAvailableOptions = await _context.School
                .AsNoTracking()
                .Select(x => new DropDownItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            return new InitUpdateProfessorResponse
            {
                Professor = professor!,
                SchoolOptions = schoolAvailableOptions
            };
        }
    }
}
