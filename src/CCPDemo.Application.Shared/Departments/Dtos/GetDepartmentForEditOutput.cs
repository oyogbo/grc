using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Departments.Dtos
{
    public class GetDepartmentForEditOutput
    {
        public CreateOrEditDepartmentDto Department { get; set; }

    }
}