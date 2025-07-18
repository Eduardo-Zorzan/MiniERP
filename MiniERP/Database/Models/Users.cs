using SQLite;
using System.ComponentModel.DataAnnotations;

namespace MiniERP.Database.Models
{
	public class User
	{
		[Required, NotNull, PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Required, NotNull]
		public string Login { get; set; }
		[Required, NotNull]
		public string Name { get; set; }
		[Required, NotNull]
		public string Password { get; set; }
	}
}
