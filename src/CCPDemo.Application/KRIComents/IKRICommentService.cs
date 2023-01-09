using CCPDemo.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KRIComents
{
    public interface IKRICommentService
    {
        bool AddComment(string comment, int KRIId);
    }
}
