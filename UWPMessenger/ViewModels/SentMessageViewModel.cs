using System;
using System.ComponentModel.DataAnnotations;

namespace UWPMessenger.ViewModels
{
    public class SentMessageViewModel
    {
        public DateTime CreatedAt { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime? ConfirmationReceivedAt { get; set; }
    }
}
