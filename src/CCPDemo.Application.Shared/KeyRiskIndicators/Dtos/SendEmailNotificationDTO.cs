using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class SendEmailNotificationDTO
    {
        public SendEmailNotificationDTO()
        {
            Recipients = new List<string>();
        }
        public List<string> Recipients { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }

    }
}
