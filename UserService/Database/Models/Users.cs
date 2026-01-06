using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models
{
	public sealed class Users
	{
		[Required, Key]
		public int Id { get; set; }
		[Required]
		public required string Email { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public required string Password { get; set; }
		public byte[]? ProfileImage { get; set; }
	}
}
