using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading.Tasks;

namespace UWPMessenger.Services
{
    public class TwilioService
    {
        private readonly string _accountSID;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public TwilioService(string accountSID, string authToken, string fromPhoneNumber)
        {
            _accountSID = accountSID;
            _authToken = authToken;
            _fromPhoneNumber = fromPhoneNumber;

            TwilioClient.Init(_accountSID, _authToken);
        }

        public async Task<string> SendMessageAsync(string to, string message)
        {
            var messageResource = await MessageResource.CreateAsync(
                to: new PhoneNumber(to),
                from: new PhoneNumber(_fromPhoneNumber),
                body: message
            );

            return messageResource.Sid;
        }
    }
}
