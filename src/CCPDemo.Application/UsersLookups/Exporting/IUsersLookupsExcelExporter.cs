using System.Collections.Generic;
using CCPDemo.UsersLookups.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.UsersLookups.Exporting
{
    public interface IUsersLookupsExcelExporter
    {
        FileDto ExportToFile(List<GetUsersLookupForViewDto> usersLookups);
    }
}