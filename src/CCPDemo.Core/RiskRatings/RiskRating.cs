using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskRatings
{
    [Table("RiskRatings")]
    public class RiskRating : Entity
    {

        [Required]
        public virtual string Name { get; set; }

        public virtual string Color { get; set; }

    }
}