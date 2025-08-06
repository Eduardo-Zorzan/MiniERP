using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using DotNetEnv;

namespace API.Database
{
	public class Context : DbContext
	{
		public DbSet<Models.Users> Users => Set<Models.Users>();

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			Env.Load();

			string ? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
			?? throw new InvalidOperationException("Database connection string is not set.");
			options.UseSqlServer(connectionString);
		}
	}
}
