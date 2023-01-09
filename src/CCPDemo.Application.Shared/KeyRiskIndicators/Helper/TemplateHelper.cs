using CCPDemo.KeyRiskIndicators.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CCPDemo.KeyRiskIndicators.Helper
{
    public static class TemplateHelper
    {
        public static string CreateAddKRITemplate(string message)
        {
            string script = $@"
                               <div style='padding:5%;'>
                                    <h4>Hello,</h4>
                                 <p>{message}</p>
                                 <p>
                                    <div>1st Floor, Nigerian Exchange Group House: 2/4 Customs Street, P.O.Box 3168, Marina, Lagos</div>
                                    <div><i class=""bi bi-telephone-fill""></i> +23414480500 Ext:</div>
                                 </p>
                                    <img src='wwwroot\Images\cscslogo.png'/>
                                </div>
                            ";
            string scriptWithHeader = "<html>" + "<body>" + script + "</body>" + "</html>";
            return scriptWithHeader;
        }
    }
}
