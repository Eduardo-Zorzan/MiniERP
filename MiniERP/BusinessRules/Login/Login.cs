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
			Database.Models.User user = await userService.GetUser(_user.Email);

			var userResult = await API.Login.LoginV1.Login(_user);

			if (userResult == null)
				throw new Exception("User not found");

			if (user != null)
			{
				user.Token = userResult.Token ?? "";
				user.Name = userResult.Name ?? user.Name;
				user.ProfileImg = userResult.ProfileImage; 

				await userService.UpdateUser(user);
			}
			else
			{
				user = new Database.Models.User
				{
					Email = userResult.Email,
					Token = userResult.Token ?? "",
					Name = userResult.Name ?? "",
					ProfileImg = userResult.ProfileImage
				};

				await userService.AddUser(user);
			}

		}

		private void Validate()
		{
			if (_user == null)
				throw new Exception("User not found.");

			if (string.IsNullOrWhiteSpace(_user.Email))
				throw new Exception("Login is blank.");

			if (string.IsNullOrWhiteSpace(_user.Password))
				throw new Exception("Password is blank");
		}
	}
}
