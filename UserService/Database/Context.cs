using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using UserService.Database.Models;

namespace UserService.Database
{
	public class Context : DbContext
	{
		public DbSet<Users> Users => Set<Users>();

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			Env.Load();

			string ? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
			?? throw new InvalidOperationException("Database connection string is not set.");
			options.UseSqlServer(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("dbo"); 
		}
	}
}
