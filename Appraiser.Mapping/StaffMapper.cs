using Appraiser.Data.Models;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public class StaffMapper : Mapper<StaffDTO, Staff>
    {
        public StaffMapper(ChangeChecker<StaffDTO> checker) : base(checker)
        {

        }

        public override Staff ToModel(StaffDTO from, Staff to)
        {
            if (to == null)
                return null;

            _checker.Check("Logon", x => x.Logon, x => x.Logon, (f, t) => t.Logon = f.Logon, from, to);
            _checker.Check("Name", x => x.Name, x => x.Name, (f, t) => t.Name = f.Name, from, to);
            _checker.Check("Email", x => x.Email, x => x.Email, (f, t) => t.Email = f.Email, from, to);
            _checker.Check("Manager Id", x => x.ManagerId, x => x.ManagerId, (f, t) => t.ManagerId = f.ManagerId, from, to);

            return to;
        }

        public override StaffDTO ToDTO(Staff from, StaffDTO to)
        {
            to.Id        = from.Id;
            to.Logon     = from.Logon;
            to.Name      = from.Name;
            to.Email     = from.Email;
            to.ManagerId = from.ManagerId;

            if (from.Manager != null)
                to.Manager = ToDTO(from.Manager, to.Manager ?? new StaffDTO());

            return to;
        }
    }
}