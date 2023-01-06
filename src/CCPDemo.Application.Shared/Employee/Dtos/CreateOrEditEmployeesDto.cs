using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Employee.Dtos
{
    public class CreateOrEditEmployeesDto : EntityDto<int?>
    {

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(EmployeesConsts.MaxLastNameLength, MinimumLength = EmployeesConsts.MinLastNameLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(EmployeesConsts.MaxFirstNameLength, MinimumLength = EmployeesConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(EmployeesConsts.MaxTitleLength, MinimumLength = EmployeesConsts.MinTitleLength)]
        public string Title { get; set; }

        [StringLength(EmployeesConsts.MaxTitleOfCourtesyLength, MinimumLength = EmployeesConsts.MinTitleOfCourtesyLength)]
        public string TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(EmployeesConsts.MaxAddressLength, MinimumLength = EmployeesConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(EmployeesConsts.MaxCityLength, MinimumLength = EmployeesConsts.MinCityLength)]
        public string City { get; set; }

        [StringLength(EmployeesConsts.MaxRegionLength, MinimumLength = EmployeesConsts.MinRegionLength)]
        public string Region { get; set; }

        [StringLength(EmployeesConsts.MaxPostalCodeLength, MinimumLength = EmployeesConsts.MinPostalCodeLength)]
        public string PostalCode { get; set; }

        [StringLength(EmployeesConsts.MaxCountryLength, MinimumLength = EmployeesConsts.MinCountryLength)]
        public string Country { get; set; }

        [StringLength(EmployeesConsts.MaxHomePhoneLength, MinimumLength = EmployeesConsts.MinHomePhoneLength)]
        public string HomePhone { get; set; }

        [StringLength(EmployeesConsts.MaxExtensionLength, MinimumLength = EmployeesConsts.MinExtensionLength)]
        public string Extension { get; set; }

        public string Photo { get; set; }

        [StringLength(EmployeesConsts.MaxNotesLength, MinimumLength = EmployeesConsts.MinNotesLength)]
        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        [StringLength(EmployeesConsts.MaxPhotoPathLength, MinimumLength = EmployeesConsts.MinPhotoPathLength)]
        public string PhotoPath { get; set; }

    }
}