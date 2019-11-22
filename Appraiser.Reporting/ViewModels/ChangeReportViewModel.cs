using System;
using Appraiser.DTOs;

namespace Appraiser.Reporting.ViewModels
{
    public class ChangeReportViewModel
    {
        public DateTime When { get; }
        public string Username { get; }
        public string Field { get; }
        public string Old { get; }
        public string New { get; }

        public ChangeReportViewModel(ChangeDTO dto)
        {
            When     = dto.When;
            Username = dto.Username.Replace("TTINT\\", "");
            Field    = $"{dto.Table}: {dto.Field}";
            Old      = dto.Old;
            New      = dto.New;
        }
    }
}