using Abp.Domain.Repositories;
using CCPDemo.Comments;
using CCPDemo.KeyRiskIndicatorHistorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KRIComents
{
    public interface IKRICommentRepo : IRepository<Comment, int>
    {

    }
}
