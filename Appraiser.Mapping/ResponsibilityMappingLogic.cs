using System;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public static class ResponsibilityMappingLogic
    {
        public static void SetAdditionalFields(this ResponsibilityDTO dto, string username)
        {
            SetOverdue(dto);
            SetCurrentUserIsManager(dto, username);
        }

        private static void SetOverdue(ResponsibilityDTO dto)
        {
            if (dto.EmployeeSignoff.HasValue && dto.ManagerSignoff.HasValue)
                dto.Overdue = false;

            dto.Overdue = true;
        }

        private static void SetCurrentUserIsManager(ResponsibilityDTO dto, string username)
        {
            if (string.Equals(dto.Staff.Logon, username, StringComparison.OrdinalIgnoreCase))
                dto.CurrentUserIsManager = false;
            else if (string.Equals(dto.Staff.Manager?.Logon, username, StringComparison.OrdinalIgnoreCase))
                dto.CurrentUserIsManager = true;
        }
    }
}