namespace API.BussinessRules.Users.Entities
{
	public class User
	{
		public required string Login { get; set; }
		public string? Name { get; set; }
		public required string Password { get; set; }

	}
}
