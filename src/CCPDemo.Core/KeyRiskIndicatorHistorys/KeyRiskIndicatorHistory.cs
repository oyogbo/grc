using Abp.Domain.Entities;
using CCPDemo.KeyRiskIndicators;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicatorHistorys
{
    [Table("KeyRiskIndicatorHistory")]
    public class KeyRiskIndicatorHistory : Entity
    {
        public virtual string TotalRecord { get; set; }
        public virtual string Department { get; set; }
        public virtual string BussinessLine { get; set; }
        [Required]
        public virtual string ReferenceId { get; set; }
        public virtual string Status { get; set; }
        public long UserId { get; set; }
        public long OrganizationUnit { get; set; }
        public string DateCreated { get; set; }
        public bool IsReviewed { get; set; }
        public string ReviewStatus { get; set; }
    }
}
