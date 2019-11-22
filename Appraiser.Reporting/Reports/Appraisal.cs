using System.Collections.Generic;
using Appraiser.DTOs;
using Appraiser.Reporting.ViewModels;

namespace Appraiser.Reporting.Reports
{
    public partial class Appraisal
    {
        public Appraisal(AppraisalDTO dto)
        {
            InitializeComponent();

            DataSource = new List<AppraisalReportViewModel>() { new AppraisalReportViewModel(dto) };
        }
    }
}
