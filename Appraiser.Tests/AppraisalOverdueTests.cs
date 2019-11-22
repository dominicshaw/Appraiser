using System;
using Appraiser.Data;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Appraiser.Tests
{
    [TestClass]
    public class AppraisalOverdueTests
    {
        private readonly AppraisalMapper _mapper;

        public AppraisalOverdueTests()
        {
            _mapper = new AppraisalMapper(
                new ChangeChecker<AppraisalDTO>(new MockHttpContextAccessor(), "Appraisal", dto => dto.Id, new AppraiserContext()),
                new MockHttpContextAccessor(),
                new StaffMapper(new ChangeChecker<StaffDTO>(new MockHttpContextAccessor(), "Staff", dto => dto.Id, new AppraiserContext())),
                new ReviewMapper(new ChangeChecker<ReviewDTO>(new MockHttpContextAccessor(), "Review", dto => dto.Id, new AppraiserContext()),
                    new TrainingMapper(new ChangeChecker<TrainingDTO>(new MockHttpContextAccessor(), "Training", dto => dto.Id, new AppraiserContext()))),
                new ObjectiveMapper(new ChangeChecker<ObjectiveDTO>(new MockHttpContextAccessor(), "Objectives", dto => dto.Id, new AppraiserContext())));
        }
        private static Appraisal CreateAppraisal(int year, Staff staff)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            return new Appraisal {PeriodEnd = end, PeriodStart = start, Staff = staff, StaffId = staff.Id};
        }

        [TestMethod]
        public void LastMidYearIsOverdue()
        {
            var appraisal = CreateAppraisal(
                2018,
                new Staff { Id = 1, Logon = "TTINT\\shawd", Manager = null, ManagerId = null, Name = "Dominic Shaw" }
            );

            appraisal.MidYearReview.Complete = false;
            appraisal.FullYearReview.Complete = false;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            dto.SetAdditionalFields("TTINT\\shawd", new DateTime(2019, 3, 13));

            Assert.IsTrue(dto.Overdue);
        }

        [TestMethod]
        public void NextYearIsNotOverdue()
        {
            var appraisal = CreateAppraisal(
                2020,
                new Staff { Id = 1, Logon = "TTINT\\shawd", Manager = null, ManagerId = null, Name = "Dominic Shaw" }
            );

            appraisal.MidYearReview.Complete = false;
            appraisal.FullYearReview.Complete = false;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            dto.SetAdditionalFields("TTINT\\shawd", new DateTime(2019, 3, 13));

            Assert.IsFalse(dto.Overdue);
        }

        [TestMethod]
        public void MidYearIsNotOverdue()
        {
            var appraisal = CreateAppraisal(
                2019,
                new Staff { Id = 1, Logon = "TTINT\\shawd", Manager = null, ManagerId = null, Name = "Dominic Shaw" }
            );

            appraisal.MidYearReview.Complete = true;
            appraisal.FullYearReview.Complete = false;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            dto.SetAdditionalFields("TTINT\\shawd", new DateTime(2019, 11, 13));

            Assert.IsFalse(dto.Overdue);
        }

        [TestMethod]
        public void FullYearIsNotOverdue()
        {
            var appraisal = CreateAppraisal(
                2019,
                new Staff { Id = 1, Logon = "TTINT\\shawd", Manager = null, ManagerId = null, Name = "Dominic Shaw" }
            );

            appraisal.MidYearReview.Complete = true;
            appraisal.FullYearReview.Complete = false;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            dto.SetAdditionalFields("TTINT\\shawd", new DateTime(2019, 11, 13));

            Assert.IsFalse(dto.Overdue);
        }

        [TestMethod]
        public void MidYearIsOverdue()
        {
            var appraisal = CreateAppraisal(
                2019,
                new Staff { Id = 1, Logon = "TTINT\\shawd", Manager = null, ManagerId = null, Name = "Dominic Shaw" }
            );

            appraisal.MidYearReview.Complete = false;
            appraisal.FullYearReview.Complete = false;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            dto.SetAdditionalFields("TTINT\\shawd", new DateTime(2019, 11, 13));

            Assert.IsTrue(dto.Overdue);
        }

        [TestMethod]
        public void LastYearIsOverdue()
        {
            var appraisal = CreateAppraisal(
                2018,
                new Staff { Id = 1, Logon = "TTINT\\shawd", Manager = null, ManagerId = null, Name = "Dominic Shaw" }
            );

            appraisal.MidYearReview.Complete = true;
            appraisal.FullYearReview.Complete = false;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            dto.SetAdditionalFields("TTINT\\shawd", new DateTime(2019, 11, 13));

            Assert.IsTrue(dto.Overdue);
        }
    }
}
