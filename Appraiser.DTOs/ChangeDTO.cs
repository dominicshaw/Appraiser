using System;

namespace Appraiser.DTOs
{
    public class ChangeDTO
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public string Username { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public int KeyId { get; set; }
        public string Old { get; set; }
        public string New { get; set; }
    }
}