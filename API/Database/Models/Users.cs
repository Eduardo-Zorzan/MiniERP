using System.ComponentModel.DataAnnotations;

namespace API.Database.Models
{
	public sealed class Users
	{
		[Required, Key]
		public int Id { get; set; }
		[Required]
		public required string Login { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public required string Password { get; set; }
	}
}
