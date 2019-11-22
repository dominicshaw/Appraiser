using Appraiser.DTOs;

namespace Appraiser.Reporting.ViewModels
{
    public class ObjectiveReportViewModel
    {
        public string ShortName { get; }
        public string ManagerNotes { get; }
        public string Measurement { get; }
        public string Description { get; }
        public string Achieved { get; }
        public decimal Weight { get; }

        public ObjectiveReportViewModel(ObjectiveDTO dto)
        {
            ShortName = dto.ShortName ?? " - ";
            Description = dto.Description ?? " - ";
            Measurement = dto.Measurement ?? " - ";
            ManagerNotes = dto.ManagerNotes ?? " - ";
            Weight = dto.Weight;

            if (dto.Achieved.HasValue)
            {
                if (dto.Achieved.Value == 1)
                    Achieved = "YES";
                else if (dto.Achieved.Value == 0)
                    Achieved = "NO";
                else
                    Achieved = $"PARTIAL ({dto.Achieved:0,#}%)";
            }
            else
            {
                Achieved = " - ";
            }
        }
    }
}