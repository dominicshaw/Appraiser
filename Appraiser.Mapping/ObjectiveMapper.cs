using Appraiser.Data.Models;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public class ObjectiveMapper : Mapper<ObjectiveDTO, Objective>
    {
        public ObjectiveMapper(ChangeChecker<ObjectiveDTO> checker)
            : base(checker)
        {
        }

        public override Objective ToModel(ObjectiveDTO from, Objective to)
        {
            _checker.Check("Name",          x => x.ShortName   , x => x.ShortName   , (f, t) => t.ShortName    = f.ShortName   , from, to);
            _checker.Check("Description",   x => x.Description , x => x.Description , (f, t) => t.Description  = f.Description , from, to);
            _checker.Check("Measurement",   x => x.Measurement , x => x.Measurement , (f, t) => t.Measurement  = f.Measurement , from, to);
            _checker.Check("Weight",        x => x.Weight      , x => x.Weight      , (f, t) => t.Weight       = f.Weight      , from, to);
            _checker.Check("Achieved",      x => x.Achieved    , x => x.Achieved    , (f, t) => t.Achieved     = f.Achieved    , from, to);
            _checker.Check("Manager Notes", x => x.ManagerNotes, x => x.ManagerNotes, (f, t) => t.ManagerNotes = f.ManagerNotes, from, to);

            return to;
        }

        public override ObjectiveDTO ToDTO(Objective from, ObjectiveDTO to)
        {
            to.Id           = from.Id;
            to.AppraisalId  = from.AppraisalId;
            to.ShortName    = from.ShortName;
            to.Description  = from.Description;
            to.Measurement  = from.Measurement;
            to.Weight       = from.Weight;
            to.Achieved     = from.Achieved;
            to.ManagerNotes = from.ManagerNotes;

            return to;
        }
    }
}