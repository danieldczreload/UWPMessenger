using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UWPMessenger.Data;
using UWPMessenger.Models;

namespace UWPMessenger.Services
{
    public class MessageService
    {
        private readonly TwilioService _twilioService;
        private readonly MessageAppContext _dbContext;

        public MessageService(TwilioService twilioService, MessageAppContext dbContext)
        {
            _twilioService = twilioService;
            _dbContext = dbContext;
        }

        public async Task SendAndStoreMessageAsync(string to, string content)
        {
            // Create and store the message
            var message = new Message
            {
                To = to,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            // Send the message via Twilio
            var confirmationCode = await _twilioService.SendMessageAsync(to, content);

            // Create and store the sent message
            var sentMessage = new SentMessage
            {
                MessageId = message.Id,
                SentAt = DateTime.UtcNow,
                ConfirmationCode = confirmationCode
            };

            _dbContext.SentMessages.Add(sentMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _dbContext.Messages
                .Include(m => m.SentMessages)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
