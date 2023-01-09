using System.Collections.Generic;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class GetKeyRiskIndicatorForViewDto
    {
        public KeyRiskIndicatorDto KeyRiskIndicator { get; set; }

        public List<KeyRiskIndicatorDto> KeyRiskIndicators { get; set; } = new List<KeyRiskIndicatorDto>();

        public string Role { get; set; }
    }
}