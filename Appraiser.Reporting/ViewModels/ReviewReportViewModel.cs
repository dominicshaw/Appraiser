using System.Collections.Generic;
using System.Linq;
using Appraiser.Common;
using Appraiser.DTOs;

namespace Appraiser.Reporting.ViewModels
{
    public class ReviewReportViewModel
    {
        public string ReviewTypeName { get; }
        public string EmployeeNotes { get; }
        public string ManagerNotes { get; }
        public List<TrainingReportViewModel> TrainingRequired { get; }
        public List<TrainingReportViewModel> TrainingAttended { get; }

        public ReviewReportViewModel(ReviewDTO dto, string type)
        {
            ReviewTypeName = type;
            EmployeeNotes = dto.EmployeeNotes ?? " - ";
            ManagerNotes = dto.ManagerNotes ?? " - ";
            TrainingRequired = new List<TrainingReportViewModel>(dto.Training.Where(x => x.TrainingType == TrainingTypes.Required).Select(x => new TrainingReportViewModel(x)));
            TrainingAttended = new List<TrainingReportViewModel>(dto.Training.Where(x => x.TrainingType == TrainingTypes.Attended).Select(x => new TrainingReportViewModel(x)));
        }
    }
}