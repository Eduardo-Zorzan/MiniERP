using SQLite;

namespace MiniERP.Database.connection
{
	public class Database
	{
		protected SQLiteAsyncConnection _db;

		protected async Task Init<T>() where T : new()
		{
			if (_db != null)
				return;

			_db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
			await _db.CreateTableAsync<T>();
		}

	}
}
