using System.Collections.Generic;
using System.Linq;
using Appraiser.Data.Models;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public class ReviewMapper : Mapper<ReviewDTO, Review>
    {
        private readonly TrainingMapper _trainingMapper;

        public override List<Change> Changes => base.Changes.Union(_trainingMapper.Changes).ToList();

        public ReviewMapper(ChangeChecker<ReviewDTO> checker, TrainingMapper trainingMapper) 
            : base(checker)
        {
            _trainingMapper = trainingMapper;
        }

        public override Review ToModel(ReviewDTO from, Review to)
        {
            _checker.Check("Employee Notes", x => x.EmployeeNotes, x => x.EmployeeNotes, (f, t) => t.EmployeeNotes = f.EmployeeNotes, from, to);
            _checker.Check("Manager Notes", x => x.ManagerNotes, x => x.ManagerNotes, (f, t) => t.ManagerNotes = f.ManagerNotes, from, to);
            _checker.Check("Complete", x => x.Complete, x => x.Complete, (f, t) => t.Complete = f.Complete, from, to);

            var validTraining = from.Training.Select(x => x.Id).ToArray();

            to.Training.RemoveAll(x => !validTraining.Contains(x.Id));

            foreach (var trainingDTO in from.Training)
            {
                var existing = to.Training.FirstOrDefault(x => x.Id == trainingDTO.Id);

                if (trainingDTO.Id == 0 || existing == null)
                {
                    existing = _trainingMapper.ToModel(trainingDTO, new Training());
                    to.Training.Add(existing);
                }
                else
                {
                    _trainingMapper.ToModel(trainingDTO, existing);
                }
            }

            return to;
        }

        public override ReviewDTO ToDTO(Review from, ReviewDTO to)
        {
            to.Id            = from.Id;
            to.EmployeeNotes = from.EmployeeNotes;
            to.ManagerNotes  = from.ManagerNotes;
            to.Complete      = from.Complete;
            to.Training      = new List<TrainingDTO>(from.Training.Select(x => _trainingMapper.ToDTO(x, new TrainingDTO())));

            return to;
        }
    }
}