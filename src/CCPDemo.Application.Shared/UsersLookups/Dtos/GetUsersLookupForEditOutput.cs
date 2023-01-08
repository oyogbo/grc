using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.UsersLookups.Dtos
{
    public class GetUsersLookupForEditOutput
    {
        public CreateOrEditUsersLookupDto UsersLookup { get; set; }

        public string UserName { get; set; }

    }
}