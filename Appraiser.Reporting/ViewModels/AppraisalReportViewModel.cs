using System;
using System.Collections.Generic;
using System.Linq;
using Appraiser.DTOs;

namespace Appraiser.Reporting.ViewModels
{
    public class AppraisalReportViewModel
    {
        public string Employee { get; }
        public string Manager { get; }
        public DateTime PeriodStart { get; }
        public DateTime PeriodEnd { get; }
        public List<ObjectiveReportViewModel> Objectives { get; }
        public List<ReviewReportViewModel> Reviews { get; } // as a list so we can put the band anywhere


        public AppraisalReportViewModel(AppraisalDTO dto)
        {
            Employee = dto.Staff.Name ?? " - ";
            Manager = dto.Staff.Manager?.Name ?? "N/A";
            PeriodStart = dto.PeriodStart;
            PeriodEnd = dto.PeriodEnd;
            Objectives = new List<ObjectiveReportViewModel>(dto.Objectives.Select(x => new ObjectiveReportViewModel(x)));
            Reviews = new List<ReviewReportViewModel>()
            {
                new ReviewReportViewModel(dto.MidYearReview, "MID-YEAR REVIEW SUMMARY"),
                new ReviewReportViewModel(dto.FullYearReview, "FULL-YEAR REVIEW SUMMARY")
            };
        }
    }
}