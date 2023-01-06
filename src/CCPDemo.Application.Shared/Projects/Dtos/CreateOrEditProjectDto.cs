using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Projects.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxNameLength, MinimumLength = ProjectConsts.MinNameLength)]
        public string Name { get; set; }

        public DateTime? DueDate { get; set; }

        public int? ConsultantId { get; set; }

        public int? BusinessOwnerId { get; set; }

        public int? DataClassificationId { get; set; }

        public int Order { get; set; }

        [Required]
        public int status { get; set; }

    }
}