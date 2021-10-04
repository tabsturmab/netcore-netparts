using System;
using System.ComponentModel.DataAnnotations;

namespace NetParts.Models
{
    public class LogEvent
    {
        [Key]
        public int Id { get; set; }
        public int? EventId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
