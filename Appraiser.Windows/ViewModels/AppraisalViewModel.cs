using System;
using System.Collections.Generic;
using Appraiser.DTOs;

namespace Appraiser.Windows.ViewModels
{
    public class AppraisalViewModel : ViewModelBase
    {
        private readonly AppraisalDTO _model;

        public int Id => _model.Id;
        public StaffDTO Staff => _model.Staff;
        public ReviewDTO MidYearReview => _model.MidYearReview;
        public ReviewDTO FullYearReview => _model.FullYearReview;
        public DateTime PeriodStart => _model.PeriodStart;
        public DateTime PeriodEnd => _model.PeriodEnd;
        public bool ObjectivesLocked => _model.ObjectivesLocked;
        public bool Overdue => _model.Overdue;
        public bool Complete => _model.Complete;

        public List<ObjectiveDTO> Objectives => _model.Objectives;

        public AppraisalViewModel(AppraisalDTO dto)
        {
            _model = dto;
        }
    }
}