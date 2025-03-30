
// Type: GameManager.Database.SQLiteDatabaseProvider
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Data.Sqlite;
using System.Data;
//using System.Data.SQLite;


namespace GameManager.Database
{
  public class SQLiteDatabaseProvider
  {
    private readonly string connectionString;

    public SQLiteDatabaseProvider(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
      return (IDbConnection) new SqliteConnection(this.connectionString);
    }
  }
}
