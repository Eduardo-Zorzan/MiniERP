using MiniERP.BusinessRules.Login.Entities;
using MiniERP.Database.Services.Users;

namespace MiniERP.BusinessRules.Login
{
	public class Login
	{
		private User _user;

		public Login(User user)
		{
			_user = user;
		}
		public async Task TryLogin()
		{
			Validate();

			var userService = new UserService();
			Database.Models.User user = await userService.GetUser(_user.Login);

			var userResult = await API.User.UserV1.Login(_user, user?.Token ?? "");

			if (userResult == null)
				throw new Exception("User not found");

			if (user != null)
			{
				user.Token = userResult.Token ?? "";
				user.Name = userResult.Name ?? user.Name;
			}
			else
			{
				user = new Database.Models.User
				{
					Email = _user.Login,
					Token = _user.Token ?? "",
					Name = _user.Name ?? ""
				};

			}

			await userService.UpdateUser(user);
		}

		private void Validate()
		{
			if (_user == null)
				throw new Exception("User not found.");

			if (string.IsNullOrWhiteSpace(_user.Login))
				throw new Exception("Login is blank.");

			if (string.IsNullOrWhiteSpace(_user.Password))
				throw new Exception("Password is blank");

		}
	}
}
