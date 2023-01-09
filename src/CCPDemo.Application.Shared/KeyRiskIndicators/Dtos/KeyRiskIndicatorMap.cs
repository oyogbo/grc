using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    sealed class KeyRiskIndicatorMap : ClassMap<KeyRiskIndicatorDto>
    {
        public KeyRiskIndicatorMap() {

            Map(m => m.BusinessLines).Name("Business Lines");
            Map(m => m.Activity).Name("Activity");
            Map(m => m.Process).Name("Process");
            Map(m => m.SubProcess).Name("Sub-Process");
            Map(m => m.PotentialRisk).Name("Potential Risk");
            Map(m => m.LikelihoodOfOccurrence_irr).Name("Likelihood Of Occurrence 1");
            Map(m => m.LikelihoodOfImpact_irr).Name("Likelihood Of Impact 1");
            Map(m => m.KeyControl).Name("Key Control");
            Map(m => m.IsControlInUse).Name("Is the control in use (Yes / No)");
            Map(m => m.BusinessLines).Name("BusinessLines");
            Map(m => m.BusinessLines).Name("BusinessLines");
            Map(m => m.BusinessLines).Name("BusinessLines");
        }
    }
}
