using CCPDemo.KeyRiskIndicators.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicators.Service.Interface
{
    public interface IEmailService
    {
        Task <bool> SendEmailAsync(SendEmailNotificationDTO email);
        void SendEmailAUsingGamil(SendEmailNotificationDTO email);
    }
}
