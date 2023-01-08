using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.UsersLookups.Dtos
{
    public class UsersLookupDto : EntityDto
    {
        public long User { get; set; }

        public long? UserId { get; set; }

    }
}