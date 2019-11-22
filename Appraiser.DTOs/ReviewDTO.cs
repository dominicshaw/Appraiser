using System.Collections.Generic;

namespace Appraiser.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string EmployeeNotes { get; set; }
        public string ManagerNotes { get; set; }
        public List<TrainingDTO> Training { get; set; } = new List<TrainingDTO>();
        public bool Complete { get; set; }
    }
}