using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.KeyRiskIndicators.Helper;
using CCPDemo.KeyRiskIndicators.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCPDemo.Authorization.Users;


namespace CCPDemo.KeyRiskIndicators.Service.Implementation
{
    public  class KRIService : IKRIService
    {
        private readonly IEmailService _emailService;
        private readonly IKeyRiskIndicatorsAppService _keyRiskIndicatorsAppService;

        public KRIService(IEmailService emailService, IKeyRiskIndicatorsAppService keyRiskIndicatorsAppService)
        {
            _emailService = emailService;
            _keyRiskIndicatorsAppService = keyRiskIndicatorsAppService; 
        }
        public async Task<bool> SendAddKRIEmailNotification()
        {
            SendEmailNotificationDTO messageModel = new SendEmailNotificationDTO();

            var recipients = await _keyRiskIndicatorsAppService.GetERMEmails();

            messageModel.HtmlContent =  TemplateHelper.CreateAddKRITemplate("A new Key Risk Indicator have been added please do well to review it");
            messageModel.Subject = "New KRI Notification";
            messageModel.Recipients = recipients;
            bool response = await _emailService.SendEmailAsync(messageModel);
            return response;
        }

         public async Task<bool> ChangeKRIStatusEmailNotificationAsync(List<string> userEmail, string RefereceId, string status)
        {
            SendEmailNotificationDTO messageModel = new SendEmailNotificationDTO();

            messageModel.HtmlContent =  TemplateHelper.CreateAddKRITemplate($@"This message is to notify you that Your KRI With the {RefereceId} has been {status}");
            messageModel.Subject = $@"KRI {status}";
            messageModel.Recipients = userEmail;
            bool response = await _emailService.SendEmailAsync(messageModel);
            return response;
        }
    }
}
