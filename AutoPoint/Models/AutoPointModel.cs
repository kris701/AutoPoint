using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoBI.AutoPoint.Models
{
	public class AutoPointModel
	{
		public List<string> Includes { get; set; }
		public BranchDefinition Branch { get; set; }
	}
}
