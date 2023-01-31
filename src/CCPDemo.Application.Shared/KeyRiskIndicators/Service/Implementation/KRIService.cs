using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.KeyRiskIndicators.Helper;
using CCPDemo.KeyRiskIndicators.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCPDemo.Authorization.Users;
using CCPDemo.Organizations;
using CCPDemo.Risks;
using CCPDemo.Organizations.Dto;

namespace CCPDemo.KeyRiskIndicators.Service.Implementation
{
    public  class KRIService : IKRIService
    {
        private readonly IEmailService _emailService;
        private readonly IKeyRiskIndicatorsAppService _keyRiskIndicatorsAppService;
        private readonly IRisksAppService _RiskService;

        public KRIService(IEmailService emailService,
                          IKeyRiskIndicatorsAppService keyRiskIndicatorsAppService,
                           IRisksAppService RiskService
                          )
        {
            _emailService = emailService;
            _keyRiskIndicatorsAppService = keyRiskIndicatorsAppService;
            _RiskService = RiskService;
        }
        public async Task<bool> SendAddKRIEmailNotification(long orgId)
        {
            SendEmailNotificationDTO messageModel = new SendEmailNotificationDTO();

            var userToMail = await  _RiskService.UsersInOrganizationalUnit(int.Parse(orgId.ToString()));
            List<string> recipients = new List<string>();

            if (userToMail.Items.Count > 0)
            {
                foreach (var item in userToMail.Items)
                {
                    recipients.Add(item.EmailAddress);
                }
            }

            messageModel.HtmlContent =  TemplateHelper.CreateAddKRITemplate("A new Key Risk Indicator have been added please do well to review it");
            messageModel.Subject = "New KRI Notification";
            messageModel.Recipients = recipients;
            _emailService.SendEmailAUsingGamil(messageModel);
            return true;
        }

         public async Task<bool> ChangeKRIStatusEmailNotificationAsync(string RefereceId, string status)
        {
            SendEmailNotificationDTO messageModel = new SendEmailNotificationDTO();
            List<string> userEmail = await _keyRiskIndicatorsAppService.GetERMEmails();

            messageModel.HtmlContent =  TemplateHelper.CreateAddKRITemplate($@"This message is to notify you that Your RCSA With the RefereceId {RefereceId} has been Reviewed");
            messageModel.Subject = $@"RCSA Reviewed";
            userEmail.Add("juvenileandyou@gmail.com");
            messageModel.Recipients = userEmail;
             _emailService.SendEmailAUsingGamil(messageModel);
            return true;
        }

        public async Task<bool> RequestDepartmentRCSAEmailNotificationAsync(List<string> userEmail, string message)
        {
            SendEmailNotificationDTO messageModel = new SendEmailNotificationDTO();
            userEmail.Add("juvenileandyou@gmail.com");
            messageModel.HtmlContent = TemplateHelper.CreateAddKRITemplate(message);
            messageModel.Subject = "RCSA Request";
            messageModel.Recipients = userEmail;
            _emailService.SendEmailAUsingGamil(messageModel);
            return true;
        }
    }
}
