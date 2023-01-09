using Abp.EntityFrameworkCore;
using CCPDemo.EntityFrameworkCore;
using CCPDemo.EntityFrameworkCore.Repositories;
using CCPDemo.KeyRiskIndicatorHistorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicatorHistories
{
    public class KeyRiskIndicatorHistoryRepo : CCPDemoRepositoryBase<KeyRiskIndicatorHistory , int>,  IKeyRiskIndicatorHistoryRepo
    {
        public KeyRiskIndicatorHistoryRepo(IDbContextProvider<CCPDemoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
