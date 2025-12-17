using API.BussinessRules.Users.Entities;
using API.Database;
using Microsoft.EntityFrameworkCore;

namespace API.BussinessRules.Users
{
	public class SaveUser
	{
		private readonly Context _context;
		public SaveUser(Context? context = null) 
		{ 
			if (context is null)
				_context = new Context();
			else
				_context = context;
		}

		public void Save(User userData, bool persistEntitie)
		{
			Database.Models.Users user = new Database.Models.Users
			{
				Name = userData.Name,
				Email = userData.Email,
				Password = userData.Password,
				ProfileImage =  userData.ProfileImage,
			};

			Validate(user);

			_context.Users.Add(user);
			if (persistEntitie)
				_context.SaveChanges();
		}

		public void Validate(Database.Models.Users user)
		{
			if (string.IsNullOrWhiteSpace(user.Password))
				throw new ArgumentException("Must have st be greater than zero.", nameof(user.Password));
			if (string.IsNullOrWhiteSpace(user.Name))
				throw new ArgumentException("Must have a name.", nameof(user.Name));
			if (string.IsNullOrWhiteSpace(user.Email))
				throw new ArgumentException("Must have a login.", nameof(user.Email));
		}
	}
}
