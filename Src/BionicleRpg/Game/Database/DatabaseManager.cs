
// Type: GameManager.Database.DatabaseManager
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using System.Linq;
using Windows.Storage;


namespace GameManager.Database
{
  public class DatabaseManager
  {
    public static DatabaseManager Instance { get; } = new DatabaseManager();

    public Repository Repo { get; private set; }

    public SavegameData CurrentSave { get; set; } = new SavegameData();

    public bool IsNewGame { get; set; } = true;

    public void CreateRepository()
    {
        StorageFolder AppDataFolder = ApplicationData.Current.LocalFolder;

            this.Repo = new Repository(new Mapper(), new SQLiteDatabaseProvider(
          @"Data Source="+   AppDataFolder.Path  + @"\"+ @"BionicleRpgDatabase.db"));
      this.Repo.Open();
    }

    public void NewSavegame()
    {
      int worldSeed = Game1.Random.Next();
      this.Repo.AddSavegame(worldSeed, 100f, 100f, new Vector2(0.0f, 0.0f));
      this.CurrentSave.ID = this.Repo.GetAllSavegames().LastOrDefault<SavegameData>().ID;
      this.CurrentSave.WorldSeed = worldSeed;
      this.CurrentSave.Health = Player.Instance.HealthComponent.CurrentHealth;
      this.CurrentSave.Energy = Player.Instance.CombatComponent.ElementalEnergy;
      Tilemap.Instance.Seed = worldSeed;
      this.IsNewGame = false;
    }

    public void LoadSavegame(int id)
    {
      SavegameData savegame = this.Repo.FindSavegame(id);
      this.CurrentSave.ID = savegame.ID;
      this.CurrentSave.WorldSeed = savegame.WorldSeed;
      this.CurrentSave.Health = savegame.Health;
      this.CurrentSave.Energy = savegame.Energy;
      this.CurrentSave.WorldPosition = savegame.WorldPosition;
      this.IsNewGame = false;
    }

    public void SetLoadVariables()
    {
      Tilemap.Instance.Seed = this.CurrentSave.WorldSeed;
      Player.Instance.HealthComponent.CurrentHealth = this.CurrentSave.Health;
      Player.Instance.CombatComponent.ElementalEnergy = this.CurrentSave.Energy;
      Player.Instance.Transform.Position = this.CurrentSave.WorldPosition;
    }

    public void SaveGame()
    {
      this.Repo.UpdateSavegame(Player.Instance.HealthComponent.CurrentHealth, Player.Instance.CombatComponent.ElementalEnergy, Player.Instance.Transform.Position);
    }
  }
}
