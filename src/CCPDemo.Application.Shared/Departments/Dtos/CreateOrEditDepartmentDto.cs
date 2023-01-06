using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Departments.Dtos
{
    public class CreateOrEditDepartmentDto : EntityDto<int?>
    {

        public string Name { get; set; }

    }
}