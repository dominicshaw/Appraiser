using System;
using System.ComponentModel.DataAnnotations;

namespace Appraiser.Data.Models
{
    public class Responsibility
    {
        [Key]
        public int Id { get; set; }

        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        [MaxLength(100)]
        public string RoleTitle { get; set; }
        public string KeySkills { get; set; }
        public string Description { get; set; }

        public DateTime? EmployeeSignoff { get; set; }
        public DateTime? ManagerSignoff { get; set; }
    }
}