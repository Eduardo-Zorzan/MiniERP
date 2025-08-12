using API.BussinessRules.Users.Entities;
using API.Database;

namespace API.BussinessRules.Users
{
	public class SaveUser
	{
		private readonly Context _context;
		public SaveUser() 
		{ 
			if (_context is null)
				_context = new Context();
		}

		public void Save(MV_SaveUser userData, bool persistEntitie)
		{
			Database.Models.Users user = new Database.Models.Users
			{
				Name = userData.Name,
				Login = userData.Login,
				Password = userData.Password
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
				throw new ArgumentException("Must have a name,", nameof(user.Name));
			if (string.IsNullOrWhiteSpace(user.Login))
				throw new ArgumentException("Must have a name,", nameof(user.Login));
		}
	}
}
