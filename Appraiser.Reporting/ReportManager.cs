using System.Collections.Generic;
using System.IO;
using Appraiser.DTOs;
using Appraiser.Reporting.Reports;

namespace Appraiser.Reporting
{
    public class ReportManager
    {
        public MemoryStream GetAppraisal(AppraisalDTO dto)
        {
            using var report = new Appraisal(dto);

            var ms = new MemoryStream();

            report.ExportToPdf(ms);

            ms.Position = 0;

            return ms;
        }

        public MemoryStream GetAppraisalChanges(AppraisalDTO appraisal, List<ChangeDTO> changes)
        {
            using var report = new AppraisalChanges(appraisal, changes);

            var ms = new MemoryStream();

            report.ExportToHtml(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
