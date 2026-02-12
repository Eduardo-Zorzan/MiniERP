namespace MiniERP.Database.Services.Users
{
	public interface IUserService
	{
		Task InitializeAsync();
		Task<Models.User> GetUser(string login);
		Task<Models.User> GetUser(int id);
		Task<List<Models.User>> GetUsers();
		Task<int> AddUser(Models.User user);
		Task<int> UpdateUser(Models.User user);
		Task<int> DeleteUser(int id);
	}
}
