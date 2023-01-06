using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.Employee
{
    [Table("Employees")]
    public class Employees : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual int EmployeeID { get; set; }

        [Required]
        [StringLength(EmployeesConsts.MaxLastNameLength, MinimumLength = EmployeesConsts.MinLastNameLength)]
        public virtual string LastName { get; set; }

        [Required]
        [StringLength(EmployeesConsts.MaxFirstNameLength, MinimumLength = EmployeesConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [StringLength(EmployeesConsts.MaxTitleLength, MinimumLength = EmployeesConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        [StringLength(EmployeesConsts.MaxTitleOfCourtesyLength, MinimumLength = EmployeesConsts.MinTitleOfCourtesyLength)]
        public virtual string TitleOfCourtesy { get; set; }

        public virtual DateTime? BirthDate { get; set; }

        public virtual DateTime? HireDate { get; set; }

        [StringLength(EmployeesConsts.MaxAddressLength, MinimumLength = EmployeesConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(EmployeesConsts.MaxCityLength, MinimumLength = EmployeesConsts.MinCityLength)]
        public virtual string City { get; set; }

        [StringLength(EmployeesConsts.MaxRegionLength, MinimumLength = EmployeesConsts.MinRegionLength)]
        public virtual string Region { get; set; }

        [StringLength(EmployeesConsts.MaxPostalCodeLength, MinimumLength = EmployeesConsts.MinPostalCodeLength)]
        public virtual string PostalCode { get; set; }

        [StringLength(EmployeesConsts.MaxCountryLength, MinimumLength = EmployeesConsts.MinCountryLength)]
        public virtual string Country { get; set; }

        [StringLength(EmployeesConsts.MaxHomePhoneLength, MinimumLength = EmployeesConsts.MinHomePhoneLength)]
        public virtual string HomePhone { get; set; }

        [StringLength(EmployeesConsts.MaxExtensionLength, MinimumLength = EmployeesConsts.MinExtensionLength)]
        public virtual string Extension { get; set; }

        public virtual string Photo { get; set; }

        [StringLength(EmployeesConsts.MaxNotesLength, MinimumLength = EmployeesConsts.MinNotesLength)]
        public virtual string Notes { get; set; }

        public virtual int? ReportsTo { get; set; }

        [StringLength(EmployeesConsts.MaxPhotoPathLength, MinimumLength = EmployeesConsts.MinPhotoPathLength)]
        public virtual string PhotoPath { get; set; }

    }
}