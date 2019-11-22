using System.Collections.Generic;
using System.Linq;
using Appraiser.DTOs;

namespace Appraiser.Reporting.ViewModels
{
    public class AppraisalChangeReportViewModel
    {
        public string Title { get; }
        public List<ChangeReportViewModel> Changes { get; }

        public AppraisalChangeReportViewModel(AppraisalDTO appraisal, List<ChangeDTO> changes)
        {
            Title = appraisal.Staff?.Name + ", " + appraisal.PeriodStart.Year;
            Changes = changes.Select(x => new ChangeReportViewModel(x)).ToList();
        }
    }
}