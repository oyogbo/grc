using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Projects.Dtos
{
    public class GetProjectForEditOutput
    {
        public CreateOrEditProjectDto Project { get; set; }

    }
}