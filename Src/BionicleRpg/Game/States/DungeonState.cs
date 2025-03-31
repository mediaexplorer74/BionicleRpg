
// Type: GameManager.States.DungeonState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.States
{
  public class DungeonState : IState
  {
    public const int ExitTilePosX = 0;
    public const int ExitTilePosY = 22;

    private readonly Vector2 entrancePosition;

    public int Seed { get; }

    public int RemainingEnemyCount { get; set; }

    public DungeonState(DungeonEntrance entrance)
    {
      this.entrancePosition = entrance.Transform.Position;
      this.Seed = entrance.Seed;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void Enter()
    {
      GameObject.ClearAll();

      Game1.CreateWorld(50, 50); // Dungeon world if 50x50 only :)

      Tilemap.Instance.Seed = this.Seed;

      Game1.InitGameObjects();

      Tilemap.Tile tile = Tilemap.Instance.Tiles[0, 22];
      Vector2Int pos = tile.TilePos * 50;

      tile.GameObject = Tilemap.Instance.CreateTileObject
      (pos, 
          Glob.Content.Load<Texture2D>("DungeonExit"), 
          TileShadowType.Object, //RnD
          Color.Gold  
       );
       tile.GameObject.AddComponent<DungeonExit>();

       tile.Type = TileType.DungeonExit;

       Player.Instance.Transform.Position = pos;

      UIManager.Instance.ShowUIComponent(UIStateAssign.Gameplay, true);
    }

    public void Exit()
    {
      //RnD : why to clear all?
      GameObject.ClearAll();
      // Re-create world?
      Game1.CreateWorld(Game1.WorldSizeX, Game1.WorldSizeY);

      Player.Instance.Transform.Position = this.entrancePosition;

      Game1.InitGameObjects();
      
      UIManager.Instance.ShowUIComponent(UIStateAssign.Gameplay, false);
    }

    public void Update()
    {
    }
  }
}
