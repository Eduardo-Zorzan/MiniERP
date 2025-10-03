﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP.BusinessRules.Login.Entities
{
	public class User
	{
		public required string Login { get; set; }
		public string? Name { get; set; }
		public string? Password { get; set; }
		public string? Token { get; set; }
	}
}
