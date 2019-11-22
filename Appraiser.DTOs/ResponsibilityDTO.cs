using System;

namespace Appraiser.DTOs
{
    public class ResponsibilityDTO
    {
        public int Id { get; set; }

        public int StaffId { get; set; }
        public StaffDTO Staff { get; } = new StaffDTO();

        public string RoleTitle { get; set; }
        public string KeySkills { get; set; }
        public string Description { get; set; }

        public DateTime? EmployeeSignoff { get; set; }
        public DateTime? ManagerSignoff { get; set; }

        public bool Overdue { get; set; }
        public bool CurrentUserIsManager { get; set; }
    }
}