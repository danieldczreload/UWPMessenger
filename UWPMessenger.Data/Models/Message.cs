using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UWPMessenger.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Content { get; set; }

        public ICollection<SentMessage> SentMessages { get; set; }
    }
}
