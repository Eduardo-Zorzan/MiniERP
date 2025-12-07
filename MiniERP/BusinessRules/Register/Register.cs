using MiniERP.BusinessRules.Register.Entities;
using MiniERP.Database.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP.BusinessRules.Register
{
    public class Register
    {
        private User _user;
        private UserService _userService;

        public Register(User user)
        {
            _user = user;
            _userService = new UserService();
        }
        public async Task TryRegister()
        {
            await Validate();

            Database.Models.User user = new Database.Models.User
            {
                Email = _user.Email,
                Name = _user.Name ?? ""
            };

            if (_user.Edit)
            {
                await API.User.UserV1.Update(_user);
                await _userService.UpdateUser(user);
            }
            else
            {
                await API.User.UserV1.Register(_user);
                await _userService.AddUser(user);
            }

        }

        private async Task Validate()
        {
            if (_user == null)
                throw new Exception("User not found.");

            if (string.IsNullOrWhiteSpace(_user.Email))
                throw new Exception("Email is blank.");

            if (string.IsNullOrWhiteSpace(_user.Password))
                throw new Exception("Password is blank");

            if (string.IsNullOrWhiteSpace(_user.ConfirmPassword))
                throw new Exception("Confirm Password is blank");

            if (!_user.Password.Equals(_user.ConfirmPassword))
                throw new Exception("Password and Confirm Password do not match.");

            if (await _userService.GetUser(_user.Email) != null)
                throw new Exception("Email already registered");
        }
    }
}
