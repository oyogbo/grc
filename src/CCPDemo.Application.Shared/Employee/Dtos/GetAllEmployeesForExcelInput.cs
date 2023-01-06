using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Employee.Dtos
{
    public class GetAllEmployeesForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxEmployeeIDFilter { get; set; }
        public int? MinEmployeeIDFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string FirstNameFilter { get; set; }

        public string TitleFilter { get; set; }

        public string TitleOfCourtesyFilter { get; set; }

        public DateTime? MaxBirthDateFilter { get; set; }
        public DateTime? MinBirthDateFilter { get; set; }

        public DateTime? MaxHireDateFilter { get; set; }
        public DateTime? MinHireDateFilter { get; set; }

        public string AddressFilter { get; set; }

        public string CityFilter { get; set; }

        public string RegionFilter { get; set; }

        public string PostalCodeFilter { get; set; }

        public string CountryFilter { get; set; }

        public string HomePhoneFilter { get; set; }

        public string ExtensionFilter { get; set; }

        public string PhotoFilter { get; set; }

        public string NotesFilter { get; set; }

        public int? MaxReportsToFilter { get; set; }
        public int? MinReportsToFilter { get; set; }

        public string PhotoPathFilter { get; set; }

    }
}