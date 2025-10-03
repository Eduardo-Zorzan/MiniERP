namespace MiniERP.Database.Services.Users
{
	public interface IUserService
	{
		Task InitializeAsync();
		Task<Models.User> GetUser(string login);
		Task<int> AddUser(Models.User user);
		Task<int> UpdateUser(Models.User user);
		Task<int> DeleteUser(Models.User user);
	}
}
