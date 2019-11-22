using System.Collections.Generic;
using System.Linq;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Microsoft.AspNetCore.Http;

namespace Appraiser.Mapping
{
    public class AppraisalMapper : Mapper<AppraisalDTO, Appraisal>
    {
        private readonly string _username;
        private readonly StaffMapper _staffMapper;
        private readonly ReviewMapper _reviewMapper;
        private readonly ObjectiveMapper _objectiveMapper;

        public override List<Change> Changes => base.Changes
            .Union(_staffMapper.Changes)
            .Union(_reviewMapper.Changes)
            .Union(_objectiveMapper.Changes)
            .ToList();

        public AppraisalMapper(
            ChangeChecker<AppraisalDTO> checker,
            IHttpContextAccessor httpContextAccessor,
            StaffMapper staffMapper,
            ReviewMapper reviewMapper,
            ObjectiveMapper objectiveMapper)
            : base(checker)
        {
            _username        = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            _staffMapper     = staffMapper;
            _reviewMapper    = reviewMapper;
            _objectiveMapper = objectiveMapper;
        }

        public override Appraisal ToModel(AppraisalDTO from, Appraisal to)
        {
            _checker.Check("Staff Id"     , x => x.StaffId         , x => x.StaffId         , (f, t) => t.StaffId          = f.StaffId         , from, to);
            _checker.Check("M-Y Review"   , x => x.MidYearReviewId , x => x.MidYearReviewId , (f, t) => t.MidYearReviewId  = f.MidYearReviewId , from, to);
            _checker.Check("F-Y Review"   , x => x.FullYearReviewId, x => x.FullYearReviewId, (f, t) => t.FullYearReviewId = f.FullYearReviewId, from, to);
            _checker.Check("Period Start" , x => x.PeriodStart     , x => x.PeriodStart     , (f, t) => t.PeriodStart      = f.PeriodStart     , from, to);
            _checker.Check("Period End"   , x => x.PeriodEnd       , x => x.PeriodEnd       , (f, t) => t.PeriodEnd        = f.PeriodEnd       , from, to);
            _checker.Check("Locked"       , x => x.ObjectivesLocked, x => x.ObjectivesLocked, (f, t) => t.ObjectivesLocked = f.ObjectivesLocked, from, to);
            _checker.Check("Complete"     , x => x.Complete        , x => x.Complete        , (f, t) => t.Complete         = f.Complete        , from, to);
            _checker.Check("Deleted"      , x => x.Deleted         , x => x.Deleted         , (f, t) => t.Deleted          = f.Deleted         , from, to);
            
            _staffMapper.ToModel(from.Staff, to.Staff);
            _reviewMapper.ToModel(from.MidYearReview, to.MidYearReview);
            _reviewMapper.ToModel(from.FullYearReview, to.FullYearReview);

            var validObjectives = from.Objectives.Select(x => x.Id).ToArray();

            to.Objectives.RemoveAll(x => !validObjectives.Contains(x.Id));

            foreach (var objDTO in from.Objectives)
            {
                var existing = to.Objectives.FirstOrDefault(x => x.Id == objDTO.Id);

                if (objDTO.Id == 0 || existing == null)
                    to.Objectives.Add(_objectiveMapper.ToModel(objDTO, new Objective()));
                else
                    _objectiveMapper.ToModel(objDTO, existing);
            }

            return to;
        }

        public override AppraisalDTO ToDTO(Appraisal from, AppraisalDTO to)
        {
            to.Id               = from.Id;
            to.StaffId          = from.StaffId;
            to.PeriodStart      = from.PeriodStart;
            to.PeriodEnd        = from.PeriodEnd;
            to.ObjectivesLocked = from.ObjectivesLocked;
            to.MidYearReviewId  = from.MidYearReviewId;
            to.FullYearReviewId = from.FullYearReviewId;
            to.Objectives       = new List<ObjectiveDTO>(from.Objectives.Select(x => _objectiveMapper.ToDTO(x, new ObjectiveDTO())));
            to.Complete         = from.Complete;
            to.Deleted          = from.Deleted;

            _staffMapper.ToDTO(from.Staff, to.Staff);
            _reviewMapper.ToDTO(from.FullYearReview, to.FullYearReview);
            _reviewMapper.ToDTO(from.MidYearReview, to.MidYearReview);

            to.SetAdditionalFields(_username);

            return to;
        }
    }
}
