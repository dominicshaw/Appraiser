
namespace Appraiser.DTOs
{
    public class StaffDTO
    {
        public int Id { get; set; }
        public string Logon { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? ManagerId { get; set; }
        public StaffDTO Manager { get; set; }
    }
}