using System.Collections.Generic;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class GetKeyRiskIndicatorForViewDto
    {
        public KeyRiskIndicatorDto KeyRiskIndicator { get; set; }

        public List<KeyRiskIndicatorDto> KeyRiskIndicators { get; set; } = new List<KeyRiskIndicatorDto>();
        public bool IsAdmin { get; set; }
        public bool IsERM { get; set; }
    }
}