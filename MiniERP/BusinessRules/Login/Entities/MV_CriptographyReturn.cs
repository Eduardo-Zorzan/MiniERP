using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP.BusinessRules.Login.Entities
{
	public class MV_CriptographyReturn
	{
		public required byte[] Key { get; set; }
		public required byte[] Iv { get; set; }
		public required string Output { get; set; }
	}
}
