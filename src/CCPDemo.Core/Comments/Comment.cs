using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.Comments
{
    [Table("Comment")]
    public class Comment : Entity
    {
        public string UserId { get; set; }
        public string CommentText { get; set; }
        public string DateCreated { get; set; }
        public int  KRIId { get; set; }
    }
}
