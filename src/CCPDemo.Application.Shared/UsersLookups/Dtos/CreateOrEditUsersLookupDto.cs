using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.UsersLookups.Dtos
{
    public class CreateOrEditUsersLookupDto : EntityDto<int?>
    {

        public long User { get; set; }

        public long? UserId { get; set; }

    }
}