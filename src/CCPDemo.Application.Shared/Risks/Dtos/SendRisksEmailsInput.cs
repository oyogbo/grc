using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.Risks.Dtos
{
    public  class SendRisksEmailsInput
    {
        public string[] EmailAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
