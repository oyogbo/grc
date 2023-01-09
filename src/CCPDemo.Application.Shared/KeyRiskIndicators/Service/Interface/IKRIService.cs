using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicators.Service.Interface
{
    public interface IKRIService
    {
        Task<bool> SendAddKRIEmailNotification();
         Task<bool> ChangeKRIStatusEmailNotificationAsync(List<string> userEmail, string RefereceId, string status);
    }
}
