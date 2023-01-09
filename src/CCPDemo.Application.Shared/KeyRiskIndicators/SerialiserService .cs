using CCPDemo.KeyRiskIndicators.Service.Interface;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace CCPDemo.KeyRiskIndicators
{
    public class SerialiserService : ISerialiserService
    {

       
        public async Task<List<RCSAModel>> ReadFileAsync(string path)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            List<RCSAModel> objToReturn = new List<RCSAModel>();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;
                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 1; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    if (rowNum <= 3)
                    {
                        continue;
                    }
                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    sb.AppendLine(string.Join(",", row));
                    var rowToList = row.ToList();
                    if (rowToList.All(x => x == "" || x == null || x == ""))
                    {
                        break;
                    }
                    RCSAModel rCSAModel = new RCSAModel();
                    rCSAModel.BussinessLines = rowToList[0];
                    rCSAModel.Activity = rowToList[1];
                    rCSAModel.Proccess = rowToList[2];
                    rCSAModel.SubProcess = rowToList[3];
                    rCSAModel.PotentailRisk = rowToList[4];
                    rCSAModel.LikelihoodOfOccurance_irr = rowToList[5];
                    rCSAModel.LikelihoodOfImpact_irr = rowToList[6];
                    rCSAModel.KeyControl = rowToList[7];
                    rCSAModel.IsControlInUse = rowToList[8] == "Yes" ? true : false;
                    rCSAModel.ControlOfEffectiveness = rowToList[9];
                    rCSAModel.LikelihoodOfOccurance_rrr = rowToList[10];
                    rCSAModel.LikelihoodOfImpact_rrr = rowToList[11];
                    rCSAModel.MitigationPlan = rowToList[12];
                    objToReturn.Add(rCSAModel);
                }
            }
            return objToReturn;
        }
    }
}
