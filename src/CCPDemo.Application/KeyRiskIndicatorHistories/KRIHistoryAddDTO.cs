using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicatorHistories
{
    public class KRIHistoryAddDTO
    {
        public string ReferenceId { get; set; }
        public virtual string TotalRecord { get; set; }
        public virtual string Department { get; set; }
        public virtual string BussinessLine { get; set; }
        public virtual string Status { get; set; }
        public long OrganizationUnit { get; set; }

    }
}
