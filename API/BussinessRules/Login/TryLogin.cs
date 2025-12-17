using API.BussinessRules.Cryptography;
using API.BussinessRules.Cryptography.Entities;
using API.BussinessRules.JWT;
using API.BussinessRules.Users.Entities;
using API.Database;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace API.BussinessRules.Login;

public class TryLogin
{
    private readonly Context _context;
    private readonly TokenService _tokenService;
    
    public TryLogin(Context? context = null, TokenService? tokenService = null) 
    { 
        if (context is null)
            _context = new Context();
        else
            _context = context; 
        
        if (tokenService is null)
            _tokenService = new TokenService();
        else
            _tokenService = tokenService;
    }
    
    public async Task<User> Login(User model)
    {
        var user = await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            throw new VerificationException("User or password invalid");
        
        string passwordToCheck = string.IsNullOrWhiteSpace(model.ActualPassword) ? user.Password : model.ActualPassword;

		Env.Load();

		CriptographyReturn criptography = new CriptographyReturn
        {
            Output = passwordToCheck,
            Iv = Convert.FromBase64String(Environment.GetEnvironmentVariable("EV") ?? ""),
            Key = Convert.FromBase64String(Environment.GetEnvironmentVariable("KEY") ?? "")
        };

        if (passwordToCheck.Equals(await API.BussinessRules.Cryptography.Cryptography.Decrypt(criptography)))
             throw new VerificationException("User or password invalid");

        model.Token = _tokenService.GenerateToken(model);
        model.Name = user.Name;

        return model;
    }
}