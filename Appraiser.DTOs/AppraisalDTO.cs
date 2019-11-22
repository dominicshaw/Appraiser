using System;
using System.Collections.Generic;

namespace Appraiser.DTOs
{
    public class AppraisalDTO
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public StaffDTO Staff { get; } = new StaffDTO();
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public bool ObjectivesLocked { get; set; }
        public int? MidYearReviewId { get; set; }
        public ReviewDTO MidYearReview { get;  } = new ReviewDTO();
        public int? FullYearReviewId { get; set; }
        public ReviewDTO FullYearReview { get; } = new ReviewDTO();
        public List<ObjectiveDTO> Objectives { get; set; } = new List<ObjectiveDTO>();
        public bool Complete { get; set; }
        public bool Deleted { get; set; }
        public bool Overdue { get; set; }
        public bool CurrentUserIsManager { get; set; }
    }
}