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
		private UserService _userService;

		public Delete(User user)
		{
			_user = user;
			_userService = new UserService();
		}

		public async Task TryDelete()
		{
			Validate();

			Login.Entities.User loginUser = new Login.Entities.User
			{
				Email = _user.Email,
				ActualPassword = _user.Password
			};

			var userResult = await API.Login.LoginV1.Login(loginUser);

			if (userResult == null || string.IsNullOrWhiteSpace(userResult.Token))
				throw new Exception("User not found");

			if (_userService == null)
				throw new Exception("User service not initialized");

			Database.Models.User userModel = await _userService.GetUser(_user.Email);

			if (userModel == null)
				throw new Exception("User not found");

			await _userService.DeleteUser(userModel.Id);
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