using Appraiser.Data.Models;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public class TrainingMapper : Mapper<TrainingDTO, Training>
    {
        public TrainingMapper(ChangeChecker<TrainingDTO> checker)
            : base(checker)
        {

        }

        public override Training ToModel(TrainingDTO from, Training to)
        {
            _checker.Check("Name", x => x.Name, x => x.Name, (f, t) => t.Name = f.Name, from, to);
            _checker.Check("Description", x => x.Description, x => x.Description, (f, t) => t.Description = f.Description, from, to);
            _checker.Check("Type", x => x.TrainingType, x => x.TrainingType, (f, t) => t.TrainingType = f.TrainingType, from, to);
            _checker.Check("Date", x => x.Date, x => x.Date, (f, t) => t.Date = f.Date, from, to);

            return to;
        }

        public override TrainingDTO ToDTO(Training from, TrainingDTO to)
        {
            to.Id           = from.Id;
            to.Name         = from.Name;
            to.Description  = from.Description;
            to.TrainingType = from.TrainingType;
            to.Date         = from.Date;

            if (to.ReviewId != from.ReviewId)
                to.ReviewId = from.ReviewId;

            return to;
        }
    }
}