using Microsoft.EntityFrameworkCore;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolManagementDotNet8Angular17MaterialTailwindDBContext _context;

        public StudentService(SchoolManagementDotNet8Angular17MaterialTailwindDBContext context)
        {
            _context = context;
        }

        public async Task<OperationStatusResponse> CreateStudent(CreateStudentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                Entities.Student studentDto = new() { Name = request.Name };

                _context.Add(studentDto);

                foreach (var subject in request.Subjects)
                {
                    Entities.StudentSubject studentSubjectDto = new()
                    {
                        SubjectId = subject.SubjectId,
                        IsMajor = subject.IsMajor
                    };
                    studentDto.StudentSubject.Add(studentSubjectDto);
                }

                await _context.SaveChangesAsync();

                return new OperationStatusResponse { IsSuccessful = true, Message = "Success. Student created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<DeleteStudentResponse> DeleteStudent(IdRequest request)
        {
            try
            {
                var studentDto = await _context.Student.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (studentDto is null)
                {
                    return new DeleteStudentResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = $"Student with ID {request.Id} not found."
                        },
                        GetAllStudentsResponse = await GetAllStudents()
                    };
                }

                _context.Remove(studentDto);

                int rowsChanged = await _context.SaveChangesAsync();

                if (rowsChanged > 0)
                {
                    return new DeleteStudentResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = true,
                            Message = "Success. Student deleted successfully."
                        },
                        GetAllStudentsResponse = await GetAllStudents()
                    };
                }
                else
                {
                    return new DeleteStudentResponse
                    {
                        OperationStatusResponse = new OperationStatusResponse
                        {
                            IsSuccessful = false,
                            Message = "Failure. Something went wrong."
                        },
                        GetAllStudentsResponse = await GetAllStudents()
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteStudentResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"An error occurred: {ex.Message}"
                    },
                    GetAllStudentsResponse = await GetAllStudents()
                };
            }
        }

        public async Task<GetAllStudentsResponse> GetAllStudents()
        {
            var response = await _context.Student
                .AsNoTracking()
                .Select(x => new StudentModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subjects = x.StudentSubject.Where(y => y.StudentId == x.Id).Select(y => new SubjectDto
                    {
                        Id = y.SubjectId,
                        Name = y.Subject.Name,
                        IsMajor = y.IsMajor,
                    }).ToList()
                }).ToListAsync();

            return new GetAllStudentsResponse { List = response };
        }

        public async Task<StudentModel?> GetStudentById(IdRequest request)
        {
            var response = await _context.Student
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new StudentModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subjects = x.StudentSubject.Where(y => y.StudentId == x.Id).Select(y => new SubjectDto
                    {
                        Id = y.SubjectId,
                        Name = y.Subject.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<GetStudentWithHisMajorResponse?> GetStudentWithHisMajor(IdRequest request)
        {
            var response = await _context.Student
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new GetStudentWithHisMajorResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Major = string.Join(", ", x.StudentSubject.Where(y => y.StudentId == x.Id && y.IsMajor).Select(y => y.Subject.Name))
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<SetNewMajorForStudentResponse> SetNewMajorForStudent(SetNewMajorForStudentRequest request)
        {
            var student = await _context.Student.FindAsync(request.StudentId);
            if (student == null)
            {
                return new SetNewMajorForStudentResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"Student with ID {request.StudentId} not found."
                    },
                    GetAllStudentsResponse = await GetAllStudents()
                };
            }

            var subject = await _context.Subject.FindAsync(request.NewMajorId);
            if (subject == null)
            {
                return new SetNewMajorForStudentResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = $"Subject with ID {request.NewMajorId} not found."
                    },
                    GetAllStudentsResponse = await GetAllStudents()
                };
            }

            var studentSubjectRecords = await _context.StudentSubject
                .Where(x => x.StudentId == request.StudentId)
                .ToListAsync();

            foreach (var item in studentSubjectRecords)
            {
                item.IsMajor = item.SubjectId == request.NewMajorId;
            }

            int rowsChanged = await _context.SaveChangesAsync();

            if (rowsChanged > 0)
            {
                return new SetNewMajorForStudentResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = true,
                        Message = "Success. Student Uudated successfully."
                    },
                    GetAllStudentsResponse = await GetAllStudents()
                };
            }
            else
            {
                return new SetNewMajorForStudentResponse
                {
                    OperationStatusResponse = new OperationStatusResponse
                    {
                        IsSuccessful = false,
                        Message = "Failure. Something went wrong."
                    },
                    GetAllStudentsResponse = await GetAllStudents()
                };
            }
        }

        public async Task<OperationStatusResponse> UpdateStudent(UpdateStudentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { IsSuccessful = false, Message = "Invalid name." };

            try
            {
                var studentDto = await _context.Student.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (studentDto is null)
                    return new OperationStatusResponse { IsSuccessful = false, Message = $"Student with ID {request.Id} not found." };

                studentDto.Name = request.Name;

                var rowsFromDatabase = await _context.StudentSubject.Where(x => x.StudentId == request.Id).ToListAsync();

                var rowsToDelete = getRowsToDelete(request.Subjects, rowsFromDatabase);
                var rowsToAdd = getRowsToAdd(request.Subjects, rowsFromDatabase);
                var rowsToUpdate = getRowsToUpdate(request.Subjects, rowsFromDatabase);

                if (rowsToDelete.Count > 0)
                {
                    foreach (StudentSubject row in rowsToDelete)
                    {
                        var item = rowsFromDatabase.Find(x => x.Id == row.Id);

                        if (item != null)
                            _context.Remove(item);
                    }
                }

                if (rowsToAdd.Count > 0)
                {
                    foreach (var row in rowsToAdd)
                    {
                        StudentSubject studentSubjectDTO = new()
                        {
                            StudentId = request.Id,
                            SubjectId = row.SubjectId,
                            IsMajor = row.IsMajor
                        };
                        _context.Add(studentSubjectDTO);
                    }
                }

                if (rowsToUpdate.Count > 0)
                {
                    foreach (SubjectInfo row in rowsToUpdate)
                    {
                        var item = rowsFromDatabase.Find(x => x.StudentId == request.Id && x.SubjectId == row.SubjectId);

                        if (item != null)
                        {
                            item.IsMajor = row.IsMajor;
                            _context.Update(item);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return new OperationStatusResponse { IsSuccessful = true, Message = $"Success. Student with ID {studentDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { IsSuccessful = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        private List<StudentSubject> getRowsToDelete(List<SubjectInfo> request, List<StudentSubject> databaseItems)
        {
            List<StudentSubject> listToReturn = new();

            foreach (StudentSubject item in databaseItems)
            {

                if (!request.Any(x => x.SubjectId == item.SubjectId))
                {
                    listToReturn.Add(item);
                }

            }

            return listToReturn;
        }

        private List<SubjectInfo> getRowsToAdd(List<SubjectInfo> request, List<StudentSubject> databaseItems)
        {
            List<SubjectInfo> listToReturn = new();

            foreach (SubjectInfo item in request)
            {
                if (!databaseItems.Any(x => x.SubjectId == item.SubjectId))
                {
                    listToReturn.Add(item);
                }
            }

            return listToReturn;
        }

        private List<SubjectInfo> getRowsToUpdate(List<SubjectInfo> request, List<StudentSubject> databaseItems)
        {
            List<SubjectInfo> listToReturn = new();

            foreach (SubjectInfo item in request)
            {
                if (databaseItems.Any(x => x.SubjectId == item.SubjectId && x.IsMajor != item.IsMajor))
                {
                    listToReturn.Add(item);
                }
            }

            return listToReturn;
        }

        public async Task<GetAvailableSubjectsResponse> GetAvailableSubjects()
        {
            var list = await _context.Subject.AsNoTracking().Select(x => new SubjectDto
            {
                Id = x.Id,
                Name = x.Name,
                IsMajor = false
            }).ToListAsync();

            return new GetAvailableSubjectsResponse { List = list };
        }
    }
}
