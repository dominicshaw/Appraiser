using System;
using Appraiser.DTOs;

namespace Appraiser.Reporting.ViewModels
{
    public class TrainingReportViewModel
    {
        public string Name { get; }
        public DateTime? Date { get; }
        public string Description { get; }

        public TrainingReportViewModel(TrainingDTO dto)
        {
            Name = dto.Name ?? " - ";
            Date = dto.Date;
            Description = dto.Description ?? " - ";
        }
    }
}