using Abp.EntityFrameworkCore;
using CCPDemo.Comments;
using CCPDemo.EntityFrameworkCore;
using CCPDemo.EntityFrameworkCore.Repositories;
using CCPDemo.KeyRiskIndicatorHistories;
using CCPDemo.KeyRiskIndicatorHistorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KRIComents
{
    public class KRICommentRepo : CCPDemoRepositoryBase<Comment, int>, IKRICommentRepo
    {
        public KRICommentRepo(IDbContextProvider<CCPDemoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
