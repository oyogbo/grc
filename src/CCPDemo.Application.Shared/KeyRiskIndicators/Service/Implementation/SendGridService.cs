using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.KeyRiskIndicators.Service.Interface;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicators.Service.Implementation
{
    public class SendGridService : IEmailService
    {
        private readonly IOptions<SendGridConfiguration> _sendGridConfig;
        private readonly string apiKey;
        private readonly string from;
        private readonly string userName;
        private readonly SendGridClient client;

        public SendGridService(IOptions<SendGridConfiguration> sendGridConfig)
        {
            _sendGridConfig = sendGridConfig;
            apiKey = "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";
            from = "davido@rpp.ng";
            userName = "Limestone";
            client = new SendGridClient(apiKey);


        }
      /*  public EmailService(IOptions<SendGridConfiguration> sendGridConfig)
        {
            _sendGridConfig = sendGridConfig;
            apiKey = _sendGridConfig.Value.ApiKey;
            from = _sendGridConfig.Value.From;
            userName = _sendGridConfig.Value.UserName;
            client = new SendGridClient(apiKey);

        }*/

        public async Task<bool> SendEmailAsync(SendEmailNotificationDTO emailModel)
        {
            try
            {
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(from, userName),
                    Subject = emailModel.Subject,
                    HtmlContent = emailModel.HtmlContent,
                };

                var recipients = new List<EmailAddress>();
                foreach (var recipient in emailModel.Recipients)
                {
                    recipients.Add(new EmailAddress(recipient));
                }

                msg.AddTos(recipients);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
