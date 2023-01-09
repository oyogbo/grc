using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace CCPDemo.KeyRiskIndicators
{
    public interface ISerialiserService
    {

        Task<List<RCSAModel>> ReadFileAsync(string path);

    }
}
