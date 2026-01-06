using UserService.BussinessRules.Users.Entities;
using Microsoft.EntityFrameworkCore;
using UserService.Database;

namespace UserService.BussinessRules.Users
{
	public class DeleteUser
	{
		private readonly Context _context;
		public DeleteUser(Context? context = null)
		{
			if (context is null)
				_context = new Context();
			else
				_context = context;
		}

		public async Task Delete(string login, bool persistEntites)
		{
			Database.Models.Users? user = await _context.Users.Where(x => x.Email.Equals(login)).FirstOrDefaultAsync();

			if (user is null)
				return;

			_context.Entry(user).State = EntityState.Deleted;
			_context.Users.Remove(user);

			if (persistEntites)
				await _context.SaveChangesAsync();
				
		}
	}
}
