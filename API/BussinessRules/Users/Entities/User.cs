namespace API.BussinessRules.Users.Entities
{
	public class User
	{
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string? Password { get; set; }
		public string? ActualPassword { get; set; }
		public string? Token { get; set; }
		public byte[]? ProfileImage { get; set; }
	}
}
