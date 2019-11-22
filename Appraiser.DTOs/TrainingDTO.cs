using System;
using Appraiser.Common;

namespace Appraiser.DTOs
{
    public class TrainingDTO
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TrainingTypes TrainingType { get; set; }
        public DateTime? Date { get; set; }
    }
}