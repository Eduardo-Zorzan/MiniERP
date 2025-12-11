using API.BussinessRules.Users.Entities;
using API.Database;
using Microsoft.EntityFrameworkCore;

namespace API.BussinessRules.Users;

public class UpdateUser
{
    private readonly Context _context;
    public UpdateUser(Context? context = null) 
    { 
        if (context is null)
            _context = new Context();
        else
            _context = context;
    }

    public async Task Update(User user, bool persistEntites)
    {
        Database.Models.Users? userToUpdate = await _context.Users.Where(x => x.Email.Equals(user.Login)).FirstOrDefaultAsync();
        
        if(userToUpdate is null)
            return;
        
        if (!string.IsNullOrWhiteSpace(user.Login))
            userToUpdate.Email =  user.Login;
        
        if (!string.IsNullOrWhiteSpace(user.Password))
            userToUpdate.Password = user.Password;
        
        if (!string.IsNullOrWhiteSpace(user.Name))
            userToUpdate.Name = user.Name;
        
        userToUpdate.ProfileImage = user.ProfileImage;
        
        _context.Entry(userToUpdate).State = EntityState.Modified;
        
        if (persistEntites)
            await _context.SaveChangesAsync();
    }
}