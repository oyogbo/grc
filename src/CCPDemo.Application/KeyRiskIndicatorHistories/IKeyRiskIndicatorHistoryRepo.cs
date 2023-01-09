using Abp.Domain.Repositories;
using CCPDemo.KeyRiskIndicatorHistorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicatorHistories
{
    public interface IKeyRiskIndicatorHistoryRepo : IRepository<KeyRiskIndicatorHistory, int>
    {


    }
}
