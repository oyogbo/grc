using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskStatus
{
    [Table("Status")]
    public class Status : Entity
    {

        [Required]
        public virtual string Name { get; set; }

    }
}