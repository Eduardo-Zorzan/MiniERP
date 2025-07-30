using System.Threading.Tasks;

namespace MiniERP.Database.Services.Users
{
	public class UserService : connection.Database, IUserService
	{
		public async Task InitializeAsync()
		{
			await Init<Models.User>();
		}

		public async Task<Models.User> GetUsersByLogin(string login)
		{
			await InitializeAsync();
			var user = (from users in _db.Table<Models.User>()
							  where users.Login == login
							  select users);

			return await user.FirstOrDefaultAsync();
		}

		public async Task<int> AddUser(Models.User user)
		{
			await InitializeAsync();
			return await _db.InsertAsync(user);
		}

		public async Task<int> DeleteUser(Models.User user)
		{
			return await _db.DeleteAsync(user);
		}

		public async Task<int> UpdateUser(Models.User user)
		{
			return await _db.UpdateAsync(user);
		}
	}
}
