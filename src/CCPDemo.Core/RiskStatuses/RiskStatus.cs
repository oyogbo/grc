using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskStatuses
{
    [Table("RiskStatus")]
    public class RiskStatus : Entity
    {

        [Required]
        public virtual string Name { get; set; }

    }
}