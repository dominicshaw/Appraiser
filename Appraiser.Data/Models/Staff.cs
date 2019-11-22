using System.ComponentModel.DataAnnotations;

namespace Appraiser.Data.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Logon { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int? ManagerId { get; set; }
        public Staff Manager { get; set; }
    }
}
