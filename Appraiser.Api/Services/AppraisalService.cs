using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraiser.Data;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Appraiser.Api.Services
{
    public class AppraisalService
    {
        private readonly ILogger<AppraisalService> _log;
        private readonly AppraiserContext _context;
        private readonly AppraisalMapper _mapper;
        private readonly ChangeService _changeService;
        private readonly string _username;

        public AppraisalService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<AppraisalService> log,
            AppraiserContext context,
            AppraisalMapper mapper,
            ChangeService changeService)
        {
            _log = log;
            _context = context;
            _mapper = mapper;
            _changeService = changeService;
            _username = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public async Task<IEnumerable<AppraisalDTO>> GetAll()
        {
            var results = await FullAppraisalStandardQuery()
                .Where(ap => EF.Functions.Like(_username, ap.Staff.Logon) || (ap.Staff.Manager != null && EF.Functions.Like(_username, ap.Staff.Manager.Logon)))
                .ToListAsync();

            _log.LogInformation("Found {count} appraisals for '{username}'.", results.Count, _username);

            return results.Select(a => _mapper.ToDTO(a, new AppraisalDTO()));
        }

        public async Task<(bool Found, AppraisalDTO Appraisal)> Get(int id)
        {
            var appraisals =
                await FullAppraisalStandardQuery()
                    .Where(ap => ap.Id == id && (EF.Functions.Like(_username, ap.Staff.Logon) || (ap.Staff.Manager != null && EF.Functions.Like(_username, ap.Staff.Manager.Logon))))
                    .ToListAsync();

            if (!appraisals.Any()) 
                return (false, null);

            var appraisal = appraisals[0];

            _log.LogInformation("Found appraisal id {id} for '{username}'.", appraisal?.Id, _username);

            if (appraisal != null)
                return (true, _mapper.ToDTO(appraisal, new AppraisalDTO()));

            return (false, null);

        }

        public async Task<(bool Found, IEnumerable<AppraisalDTO> Appraisal)> GetByUser()
        {
            var staff = await _context.Staff.SingleOrDefaultAsync(s => EF.Functions.Like(s.Logon, _username));

            if (staff == null)
                return (false, null);

            var appraisals = await FullAppraisalStandardQuery()
                .Where(x => x.StaffId == staff.Id)
                .ToListAsync();

            _log.LogInformation("Found {count} appraisals for user '{username}'.", appraisals.Count, _username);

            return (true, appraisals.Select(a => _mapper.ToDTO(a, new AppraisalDTO())));
        }

        public async Task<IEnumerable<AppraisalDTO>> GetByManager()
        {
            var staff = await _context.Staff.SingleOrDefaultAsync(x => EF.Functions.Like(x.Logon, _username));

            if (staff == null)
                return null;

            var appraisals = await FullAppraisalStandardQuery()
                .Where(x => x.Staff.ManagerId == staff.Id)
                .ToListAsync();

            _log.LogInformation("Found {count} appraisals for manager '{username}'.", appraisals.Count, _username);

            return appraisals.Select(a => _mapper.ToDTO(a, new AppraisalDTO()));
        }

        public async Task<bool> Update(int id, AppraisalDTO dto)
        {
            var newReview = dto.MidYearReview.Id == 0 || dto.FullYearReview.Id == 0;

            var appraisal = await FullAppraisalStandardQuery().SingleOrDefaultAsync(x => x.Id == id);

            if (appraisal == null)
                return false;

            var locked = appraisal.ObjectivesLocked;

            try
            {
                _mapper.ToModel(dto, appraisal);

                _context.Entry(appraisal).State = EntityState.Modified;

                _log.LogInformation("Saved appraisal id {id} for '{username}'.", appraisal.Id, _username);

                if (!newReview)
                {
                    await _context.SaveChangesAsync();
                    return true;
                }

                await SaveReviews(appraisal);
                return true;
            }
            finally
            {
                if (locked) await _changeService.NotifyAppraisalChange(appraisal, _mapper.Changes);
            }
        }

        private async Task SaveReviews(Appraisal appraisal)
        {
            if (appraisal.MidYearReview.Id == 0)
                _context.Reviews.Add(appraisal.MidYearReview);
            if (appraisal.FullYearReview.Id == 0)
                _context.Reviews.Add(appraisal.FullYearReview);

            await _context.SaveChangesAsync();

            if (appraisal.MidYearReview.Id != appraisal.MidYearReviewId)
                appraisal.MidYearReviewId = appraisal.MidYearReview.Id;
            if (appraisal.FullYearReview.Id != appraisal.FullYearReviewId)
                appraisal.FullYearReviewId = appraisal.FullYearReview.Id;

            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<AppraisalDTO>> Clone(int id, int year)
        {
            var appraisal = await FullAppraisalStandardQuery()
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            var (found, existing) = await ExistenceCheck(appraisal.StaffId, year);

            if (found)
                return existing;

            var dto = _mapper.ToDTO(appraisal, new AppraisalDTO());

            ResetAppraisal(dto, year);

            var cloned = new Appraisal();

            _context.Appraisals.Add(_mapper.ToModel(dto, cloned));

            await _context.SaveChangesAsync();

            return _mapper.ToDTO(cloned, dto);
        }

        private static void ResetAppraisal(AppraisalDTO dto, int year)
        {
            dto.Id = 0;
            dto.PeriodStart = new DateTime(year, 1, 1);
            dto.PeriodEnd = new DateTime(year, 12, 31);
            dto.MidYearReviewId = 0;
            dto.MidYearReview.Id = 0;
            dto.FullYearReviewId = 0;
            dto.FullYearReview.Id = 0;
            dto.Objectives.ForEach(o => o.Id = 0);
        }

        public async Task<AppraisalDTO> Save(AppraisalDTO dto)
        {
            var (found, existing) = await ExistenceCheck(dto.StaffId, dto.PeriodStart.Year);

            if (found)
                return existing;

            var appraisal = _mapper.ToModel(dto, new Appraisal());

            _context.Appraisals.Add(appraisal);

            await _context.SaveChangesAsync();

            var (_, result) = await Get(appraisal.Id);

            return result;
        }

        public async Task<(bool Found, AppraisalDTO Appraisal)> Delete(int id)
        {
            var appraisal =
                await _context.Appraisals
                    .Where(a => !a.Deleted && a.Id == id)
                    .Include(a => a.Staff).ThenInclude(s => s.Manager)
                    .SingleOrDefaultAsync(ap => EF.Functions.Like(_username, ap.Staff.Logon) || (ap.Staff.Manager != null && EF.Functions.Like(_username, ap.Staff.Manager.Logon)));

            if (appraisal == null)
                return (false, null);

            _context.Appraisals.Remove(appraisal);

            await _context.SaveChangesAsync();

            return (true, _mapper.ToDTO(appraisal, new AppraisalDTO()));
        }

        private async Task<(bool Found, AppraisalDTO Existing)> ExistenceCheck(int staffId, int year)
        {
            var existenceCheck = await FullAppraisalStandardQuery()
                .Where(a => !a.Deleted && a.StaffId == staffId && a.PeriodStart.Year == year)
                .SingleOrDefaultAsync();

            return existenceCheck != null ?
                (true, _mapper.ToDTO(existenceCheck, new AppraisalDTO())) :
                (false, null);
        }

        private IIncludableQueryable<Appraisal, List<Objective>> FullAppraisalStandardQuery()
        {
            return _context.Appraisals
                .Where(x => !x.Deleted)
                .Include(x => x.Staff).ThenInclude(x => x.Manager)
                .Include(x => x.MidYearReview).ThenInclude(x => x.Training)
                .Include(x => x.FullYearReview).ThenInclude(x => x.Training)
                .Include(x => x.Objectives);
        }
    }
}
