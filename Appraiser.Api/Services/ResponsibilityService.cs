using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraiser.Data;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Appraiser.Api.Services
{
    public class ResponsibilityService
    {
        private readonly ILogger<ResponsibilityService> _log;
        private readonly AppraiserContext _context;
        private readonly ResponsibilityMapper _mapper;
        private readonly string _username;

        public ResponsibilityService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<ResponsibilityService> log,
            AppraiserContext context,
            ResponsibilityMapper mapper)
        {
            _log = log;
            _context = context;
            _mapper = mapper;
            _username = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public async Task<List<ResponsibilityDTO>> GetAll()
        {
            return (await StandardQuery().ToListAsync()).Select(x => _mapper.ToDTO(x, new ResponsibilityDTO())).ToList();
        }

        public async Task<(bool Found, ResponsibilityDTO Responsibility)> Get(int id)
        {
            var resp = await StandardQuery().SingleOrDefaultAsync(r => r.Id == id);

            if (resp == null)
                return (false, null);

            return (true, _mapper.ToDTO(resp, new ResponsibilityDTO()));
        }

        public async Task<(bool Found, IEnumerable<ResponsibilityDTO> Responsibility)> GetByUser()
        {
            var you = await _context.Staff.SingleOrDefaultAsync(s => EF.Functions.Like(s.Logon, _username));

            if (you == null)
                return (false, null);

            var result = await StandardQuery().Where(x => x.StaffId == you.Id).ToListAsync();

            return (true, result.Select(s => _mapper.ToDTO(s, new ResponsibilityDTO())));
        }

        public async Task<bool> Update(int id, ResponsibilityDTO dto)
        {
            var resp = await StandardQuery().SingleOrDefaultAsync(x => x.Id == id);

            if (resp == null)
                return false;

            _mapper.ToModel(dto, resp);

            _context.Entry(resp).State = EntityState.Modified;

            _log.LogInformation("Saved resp id {id} for '{username}'.", resp.Id, _username);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ResponsibilityDTO> Save(ResponsibilityDTO dto)
        {
            var resp = _mapper.ToModel(dto, new Responsibility());

            _context.Responsibilities.Add(resp);

            await _context.SaveChangesAsync();

            var (_, result) = await Get(resp.Id);

            return result;
        }

        public async Task<(bool Found, ResponsibilityDTO Appraisal)> Delete(int id)
        {
            var resp = await StandardQuery().SingleOrDefaultAsync(r => r.Id == id);

            if (resp == null)
                return (false, null);

            _context.Responsibilities.Remove(resp);

            await _context.SaveChangesAsync();

            return (true, _mapper.ToDTO(resp, new ResponsibilityDTO()));
        }

        private IIncludableQueryable<Responsibility, Staff> StandardQuery()
        {
            return _context.Responsibilities
                .Include(r => r.Staff)
                .ThenInclude(s => s.Manager);
        }
    }
}