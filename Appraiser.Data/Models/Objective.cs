using System.ComponentModel.DataAnnotations;

namespace Appraiser.Data.Models
{
    public class Objective
    {
        [Key]
        public int Id { get; set; }

        public int AppraisalId { get; set; }

        [MaxLength(30)]
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Measurement { get; set; }
        public decimal Weight { get; set; }

        public decimal? Achieved { get; set; }
        public string ManagerNotes { get; set; }
    }
}