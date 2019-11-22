using Appraiser.Data.Models;
using Appraiser.DTOs;
using Microsoft.AspNetCore.Http;

namespace Appraiser.Mapping
{
    public class ResponsibilityMapper : Mapper<ResponsibilityDTO, Responsibility>
    {
        private readonly StaffMapper _staffMapper;
        private readonly string _username;

        public ResponsibilityMapper(
            ChangeChecker<ResponsibilityDTO> checker,
            IHttpContextAccessor httpContextAccessor,
            StaffMapper staffMapper) : base(checker)
        {
            _username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            _staffMapper = staffMapper;
        }

        public override Responsibility ToModel(ResponsibilityDTO from, Responsibility to)
        {
            _checker.Check("Staff Id", x => x.StaffId, x => x.StaffId, (f, t) => t.StaffId = f.StaffId, from, to);
            _checker.Check("Role Title", x => x.RoleTitle, x => x.RoleTitle, (f, t) => t.RoleTitle = f.RoleTitle, from, to);
            _checker.Check("Key Skills", x => x.KeySkills, x => x.KeySkills, (f, t) => t.KeySkills = f.KeySkills, from, to);
            _checker.Check("Description", x => x.Description, x => x.Description, (f, t) => t.Description = f.Description, from, to);
            _checker.Check("Employee Signoff", x => x.EmployeeSignoff, x => x.EmployeeSignoff, (f, t) => t.EmployeeSignoff = f.EmployeeSignoff, from, to);
            _checker.Check("Manager Signoff", x => x.ManagerSignoff, x => x.ManagerSignoff, (f, t) => t.ManagerSignoff = f.ManagerSignoff, from, to);

            _staffMapper.ToModel(from.Staff, to.Staff);

            return to;
        }

        public override ResponsibilityDTO ToDTO(Responsibility from, ResponsibilityDTO to)
        {
            to.Id              = from.Id;
            to.RoleTitle       = from.RoleTitle;
            to.KeySkills       = from.KeySkills;
            to.Description     = from.Description;
            to.StaffId         = from.StaffId;
            to.EmployeeSignoff = from.EmployeeSignoff;
            to.ManagerSignoff  = from.ManagerSignoff;

            _staffMapper.ToDTO(from.Staff, to.Staff);

            to.SetAdditionalFields(_username);

            return to;
        }
    }
}