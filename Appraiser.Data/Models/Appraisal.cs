using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Appraiser.Data.Models
{
    public class Appraisal
    {
        [Key]
        public int Id { get; set; }

        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public bool ObjectivesLocked { get; set; }

        public int? MidYearReviewId { get; set; }
        public Review MidYearReview { get; set; } = new Review();
        public int? FullYearReviewId { get; set; }
        public Review FullYearReview { get; set; } = new Review();

        public List<Objective> Objectives { get; set; } = new List<Objective>();

        public bool Complete { get; set; }
        public bool Deleted { get; set; }
    }
}