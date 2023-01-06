using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Closures.Dtos
{
    public class CreateOrEditClosureDto : EntityDto<int?>
    {

        public int RiskId { get; set; }

        public int UserId { get; set; }

        public DateTime ClosureDate { get; set; }

        public int CloseReasonId { get; set; }

        public string Note { get; set; }

    }
}