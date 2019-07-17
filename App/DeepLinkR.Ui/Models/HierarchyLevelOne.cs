using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepLinkR.Ui.Models
{
	public class HierarchyLevelOne
	{
		public string Name { get; set; }

		public List<HierarchyLevelTwo> SecondLinkHierarchies { get; set; }
	}
}
