using System;
using System.ComponentModel.DataAnnotations;

namespace Appraiser.Data.Models
{
    public class Change
    {
        [Key]
        public int Id { get; set; }
        public DateTime When { get; set; }
        [MaxLength(100)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Table { get; set; }
        [MaxLength(100)]
        public string Field { get; set; }
        public int KeyId { get; set; }
        public string Old { get; set; }
        public string New { get; set; }
    }
}