using MiniERP.BusinessRules.Register.Entities;
using MiniERP.Database.Services.Users;

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
            Validate();


            if (_user.Edit)
            {
                await API.User.UserV1.Update(_user);

                Database.Models.User user = await _userService.GetUser(_user.Email);

                user.Email = _user.Email;
                user.Name = _user.Name;
                user.ProfileImg = _user.ProfileImage;

                await _userService.UpdateUser(user);
            }
            else
            {
				Database.Models.User user = new Database.Models.User
				{
					Email = _user.Email,
					Name = _user.Name,
					ProfileImg = _user.ProfileImage
				};

				await API.User.UserV1.Register(_user);
                await _userService.AddUser(user);
            }

        }

        private void Validate()
        {
            if (_user == null)
                throw new Exception("User not found.");

            if (string.IsNullOrWhiteSpace(_user.Email))
                throw new Exception("Email is blank.");

            if (!string.IsNullOrWhiteSpace(_user.Password)
                && !_user.Password.Equals(_user.ConfirmPassword))
				throw new Exception("Password and Confirm Password do not match.");
		}
    }
}
