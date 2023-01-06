using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Employee.Dtos
{
    public class GetEmployeesForEditOutput
    {
        public CreateOrEditEmployeesDto Employees { get; set; }

    }
}