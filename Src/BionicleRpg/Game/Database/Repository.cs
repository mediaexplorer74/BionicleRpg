
// Type: GameManager.Database.Repository
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.GameObjects.Components;
using Microsoft.Data.Sqlite;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace GameManager.Database
{
  public class Repository
  {
    private readonly Mapper mapper;
    private readonly SQLiteDatabaseProvider provider;
    private IDbConnection connection;

    public Repository(Mapper mapper, SQLiteDatabaseProvider provider)
    {
      this.mapper = mapper;
      this.provider = provider;
    }

    public void CreateTables()
    {
      new SqliteCommand("CREATE TABLE IF NOT EXISTS Characters (CharacterID INTEGER PRIMARY KEY, SaveID, Name STRING, Element INTEGER, Weapon INTEGER, Mask INTEGER, FOREIGN KEY(SaveID) REFERENCES savegame(SaveID))", 
          (SqliteConnection) this.connection).ExecuteNonQuery();
      new SqliteCommand("CREATE TABLE IF NOT EXISTS Savegame (SaveID INTEGER PRIMARY KEY, WorldSeed INTEGER, Health FLOAT, Energy FLOAT, WorldPositionX FLOAT, WorldPositionY FLOAT)",
          (SqliteConnection) this.connection).ExecuteNonQuery();
    }

        // Replace all instances of DefaultInterpolatedStringHandler with StringBuilder

        public void AddSavegame(int worldSeed, float health, float energy, Vector2 worldPosition)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("INSERT INTO Savegame VALUES (null, ");
            interpolatedStringHandler.Append(worldSeed);
            interpolatedStringHandler.Append(", ");
            interpolatedStringHandler.Append(health);
            interpolatedStringHandler.Append(", ");
            interpolatedStringHandler.Append(energy);
            interpolatedStringHandler.Append(", ");
            interpolatedStringHandler.Append(worldPosition.X);
            interpolatedStringHandler.Append(", ");
            interpolatedStringHandler.Append(worldPosition.Y);
            interpolatedStringHandler.Append(")");
            new SqliteCommand(interpolatedStringHandler.ToString(), (SqliteConnection)this.connection).ExecuteNonQuery();
        }

        public void AddCharacter(string name, Element element, AttackType weapon, MaskType mask)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("INSERT INTO Characters (SaveID, Name, Element, Weapon, Mask) VALUES ((SELECT SaveID FROM Savegame WHERE SaveID = ");
            interpolatedStringHandler.Append(DatabaseManager.Instance.CurrentSave.ID);
            interpolatedStringHandler.Append("), '");
            interpolatedStringHandler.Append(name);
            interpolatedStringHandler.Append("', ");
            interpolatedStringHandler.Append((int)element);
            interpolatedStringHandler.Append(", ");
            interpolatedStringHandler.Append((int)weapon);
            interpolatedStringHandler.Append(", ");
            interpolatedStringHandler.Append((int)mask);
            interpolatedStringHandler.Append(")");
            new SqliteCommand(interpolatedStringHandler.ToString(), (SqliteConnection)this.connection).ExecuteNonQuery();
        }

        public void DeleteSavegame(int id)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("DELETE from Characters WHERE SaveID = ");
            interpolatedStringHandler.Append(id);
            new SqliteCommand(interpolatedStringHandler.ToString(), (SqliteConnection)this.connection).ExecuteNonQuery();
            interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("DELETE from Savegame WHERE SaveID = ");
            interpolatedStringHandler.Append(id);
            new SqliteCommand(interpolatedStringHandler.ToString(), (SqliteConnection)this.connection).ExecuteNonQuery();
        }

        public SavegameData FindSavegame(int id)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("SELECT * from Savegame WHERE SaveID = ");
            interpolatedStringHandler.Append(id);
            return this.mapper.MapSavegameFromReader(
                new SqliteCommand(interpolatedStringHandler.ToString(), 
                (SqliteConnection)this.connection).ExecuteReader()).FirstOrDefault<SavegameData>();
        }

    public List<SavegameData> GetAllSavegames()
    {
      return this.mapper.MapSavegameFromReader(new SqliteCommand("SELECT * from Savegame",
          (SqliteConnection) this.connection).ExecuteReader());
    }

        public void UpdateSavegame(float health, float energy, Vector2 worldPosition)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("UPDATE Savegame SET Health = ");
            interpolatedStringHandler.Append(health);
            interpolatedStringHandler.Append(", Energy = ");
            interpolatedStringHandler.Append(energy);
            interpolatedStringHandler.Append(", WorldPositionX = ");
            interpolatedStringHandler.Append(worldPosition.X);
            interpolatedStringHandler.Append(", WorldPositionY = ");
            interpolatedStringHandler.Append(worldPosition.Y);
            interpolatedStringHandler.Append(" WHERE SaveID = ");
            interpolatedStringHandler.Append(DatabaseManager.Instance.CurrentSave.ID);
            new SqliteCommand(interpolatedStringHandler.ToString(), 
                (SqliteConnection)this.connection).ExecuteNonQuery();
        }

        public void UpdateCharacterWeapon(Element element, AttackType weapon)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("UPDATE Characters SET Weapon = ");
            interpolatedStringHandler.Append((int)weapon);
            interpolatedStringHandler.Append(" WHERE Element = ");
            interpolatedStringHandler.Append((int)element);
            interpolatedStringHandler.Append(" AND SaveID = ");
            interpolatedStringHandler.Append(DatabaseManager.Instance.CurrentSave.ID);
            new SqliteCommand(interpolatedStringHandler.ToString(), (SqliteConnection)this.connection).ExecuteNonQuery();
        }

        public void UpdateCharacterMask(Element element, MaskType mask)
        {
            StringBuilder interpolatedStringHandler = new StringBuilder();
            interpolatedStringHandler.Append("UPDATE Characters SET Mask = ");
            interpolatedStringHandler.Append((int)mask);
            interpolatedStringHandler.Append(" WHERE Element = ");
            interpolatedStringHandler.Append((int)element);
            interpolatedStringHandler.Append(" AND SaveID = ");
            interpolatedStringHandler.Append(DatabaseManager.Instance.CurrentSave.ID);
            new SqliteCommand(interpolatedStringHandler.ToString(), (SqliteConnection)this.connection).ExecuteNonQuery();
        }

    public void Open()
    {
      if (this.connection == null)
        this.connection = this.provider.CreateConnection();
      this.connection.Open();
      this.CreateTables();
    }

    public void Close() => this.connection.Close();
  }
}
