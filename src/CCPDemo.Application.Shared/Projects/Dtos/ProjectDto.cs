using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.Projects.Dtos
{
    public class ProjectDto : EntityDto
    {
        public int Value { get; set; }

        public string Name { get; set; }

        public DateTime? DueDate { get; set; }

        public int? ConsultantId { get; set; }

        public int? BusinessOwnerId { get; set; }

        public int? DataClassificationId { get; set; }

        public int Order { get; set; }

        public int status { get; set; }

    }
}