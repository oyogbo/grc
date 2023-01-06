using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskTransactions
{
    [Table("RiskTransactions")]
    public class RiskTransaction : Entity
    {

        public virtual string TransactionType { get; set; }

        public virtual string Date { get; set; }

        public virtual string CurrentValue { get; set; }

        public virtual string NewValue { get; set; }

        public virtual int RiskId { get; set; }

        public virtual long UserId { get; set; }

    }
}