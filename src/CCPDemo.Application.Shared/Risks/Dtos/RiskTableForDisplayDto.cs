using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.Risks.Dtos
{
	public class RiskTableForDisplayDto
	{
		public string Summary { get; set; }
		public string OrganizationUnitDisplayName { get; set; }
		public DateTime TargetDate { get; set; }
	}
}
