using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskTypes
{
    [Table("RiskTypes")]
    public class RiskType : FullAuditedEntity
    {

        public virtual string Name { get; set; }

    }
}