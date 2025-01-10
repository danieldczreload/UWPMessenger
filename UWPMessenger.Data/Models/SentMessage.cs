using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UWPMessenger.Models
{
    public class SentMessage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public DateTime SentAt { get; set; }
        public string ConfirmationCode { get; set; }

        public Message Message { get; set; }
    }
}
