using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraiser.Data;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Appraiser.Api.Services
{
    public class StaffService
    {
        private readonly ILogger<StaffService> _log;
        private readonly AppraiserContext _context;
        private readonly StaffMapper _mapper;
        private readonly string _username;

        public StaffService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<StaffService> log,
            AppraiserContext context,
            StaffMapper mapper)
        {
            _log = log;
            _context = context;
            _mapper = mapper;
            _username = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public async Task<List<StaffDTO>> GetAll()
        {
            return (await _context.Staff.ToListAsync()).Select(x => _mapper.ToDTO(x, new StaffDTO())).ToList();
        }

        public async Task<(bool Found, StaffDTO Staff)> Get(int id)
        {
            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
                return (false, null);

            return (true, _mapper.ToDTO(staff, new StaffDTO()));
        }

        public async Task<(bool Found, IEnumerable<StaffDTO> Staff)> GetByUser()
        {
            var you = await _context.Staff.SingleOrDefaultAsync(s => EF.Functions.Like(s.Logon, _username));

            if (you == null)
                return (false, null);

            var result = await _context.Staff.Where(x => x.ManagerId == you.Id).ToListAsync();

            result.Add(you);

            return (true, result.Select(s => _mapper.ToDTO(s, new StaffDTO())));
        }

        public async Task<bool> Update(int id, StaffDTO dto)
        {
            var staff = await _context.Staff.SingleOrDefaultAsync(x => x.Id == id);

            if (staff == null)
                return false;

            _mapper.ToModel(dto, staff);

            _context.Entry(staff).State = EntityState.Modified;

            _log.LogInformation("Saved staff id {id} for '{username}'.", staff.Id, _username);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StaffDTO> Save(StaffDTO dto)
        {
            var staff = _mapper.ToModel(dto, new Staff());

            _context.Staff.Add(staff);

            await _context.SaveChangesAsync();

            var (_, result) = await Get(staff.Id);

            return result;
        }

        public async Task<(bool Found, StaffDTO Appraisal)> Delete(int id)
        {
            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
                return (false, null);

            _context.Staff.Remove(staff);

            await _context.SaveChangesAsync();

            return (true, _mapper.ToDTO(staff, new StaffDTO()));
        }
    }
}