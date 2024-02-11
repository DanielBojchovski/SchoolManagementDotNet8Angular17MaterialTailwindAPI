﻿using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Responses
{
    public class UpdateSubjectResponse
    {
        public GetAllSubjectsResponse GetAllSubjectsResponse { get; set; } = new();
        public OperationStatusResponse OperationStatusResponse { get; set; } = new();
    }
}
