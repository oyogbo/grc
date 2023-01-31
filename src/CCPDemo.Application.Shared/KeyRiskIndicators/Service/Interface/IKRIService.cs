using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicators.Service.Interface
{
    public interface IKRIService
    {
        Task<bool> SendAddKRIEmailNotification(long orgId);
        Task<bool> ChangeKRIStatusEmailNotificationAsync(string RefereceId, string status);
        Task<bool> RequestDepartmentRCSAEmailNotificationAsync(List<string> userEmail, string message);

    }
}
