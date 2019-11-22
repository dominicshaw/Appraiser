
namespace Appraiser.DTOs
{
    public class ObjectiveDTO
    {
        public int Id { get; set; }
        public int AppraisalId { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Measurement { get; set; }
        public decimal Weight { get; set; }
        public decimal? Achieved { get; set; }
        public string ManagerNotes { get; set; }
    }
}