using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Closures.Dtos
{
    public class GetClosureForEditOutput
    {
        public CreateOrEditClosureDto Closure { get; set; }

    }
}