using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.UsersLookups.Dtos
{
    public class GetAllUsersLookupsForExcelInput
    {
        public string Filter { get; set; }

        public long? MaxUserFilter { get; set; }
        public long? MinUserFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}