﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP.Database.connection
{
	public static class Constants
	{
		public const string DatabaseFilename = "database.db3";

		public const SQLite.SQLiteOpenFlags Flags =
			SQLite.SQLiteOpenFlags.ReadWrite |
			SQLite.SQLiteOpenFlags.Create |
			SQLite.SQLiteOpenFlags.SharedCache;

		public static string DatabasePath =>
			Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
	}
}
