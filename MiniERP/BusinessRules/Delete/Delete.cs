using MiniERP.BusinessRules.Delete.Entities;
using MiniERP.Database.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP.BusinessRules.Delete
{
	public class Delete
	{

		private User _user;

		public Delete(User user)
		{
			_user = user;
		}

		public async Task TryDelete()
		{
			Validate();

			var userService = new UserService();

			Login.Entities.User loginUser = new Login.Entities.User
			{
				Email = _user.Email,
				ActualPassword = _user.Password
			};

			var userResult = await API.Login.LoginV1.Login(loginUser);

			if (userResult == null || string.IsNullOrWhiteSpace(userResult.Token))
				throw new Exception("User not found");

			Database.Models.User userModel = await userService.GetUser(_user.Email);
			await userService.DeleteUser(userModel);
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