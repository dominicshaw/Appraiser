using System.Collections.Generic;
using Appraiser.DTOs;
using Appraiser.Reporting.ViewModels;

namespace Appraiser.Reporting.Reports
{
    public partial class AppraisalChanges
    {
        public AppraisalChanges(AppraisalDTO appraisal, List<ChangeDTO> changes)
        {
            InitializeComponent();
            DataSource = new List<AppraisalChangeReportViewModel> { new AppraisalChangeReportViewModel(appraisal, changes) };
        }
    }
}
