using System;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public static class AppraisalMappingLogic
    {
        public static void SetAdditionalFields(this AppraisalDTO dto, string username, DateTime? asOfDate = null)
        {
            SetOverdue(dto, asOfDate);
            SetCurrentUserIsManager(dto, username);
        }

        private static void SetOverdue(AppraisalDTO dto, DateTime? asOfDate)
        {
            if (!asOfDate.HasValue) asOfDate = DateTime.Now;

            var year = dto.PeriodStart.Year;

            var currentYear = asOfDate.Value.Year;
            var currentMonth = asOfDate.Value.Month;

            if (year > currentYear)
                dto.Overdue = false;
            else if (!dto.MidYearReview.Complete && currentYear > year)
                dto.Overdue = true;
            else if (!dto.MidYearReview.Complete && currentYear < year)
                dto.Overdue = true;
            else if (!dto.MidYearReview.Complete && currentYear == year && currentMonth >= 7)
                dto.Overdue = true;
            else if (!dto.FullYearReview.Complete && currentYear > year)
                dto.Overdue = true;
            else
                dto.Overdue = false;
        }

        private static void SetCurrentUserIsManager(AppraisalDTO dto, string username)
        {
            if (string.Equals(dto.Staff.Logon, username, StringComparison.OrdinalIgnoreCase))
                dto.CurrentUserIsManager = false;
            else if (string.Equals(dto.Staff.Manager?.Logon, username, StringComparison.OrdinalIgnoreCase))
                dto.CurrentUserIsManager = true;
        }
    }
}