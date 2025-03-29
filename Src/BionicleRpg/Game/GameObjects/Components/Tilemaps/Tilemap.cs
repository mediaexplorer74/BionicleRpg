
// Type: GameManager.GameObjects.Components.Tilemaps.Tilemap
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Builders;
using GameManager.DataTypes;
using GameManager.GameObjects.Components.Lighting;
using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Renderers;
using GameManager.GameObjects.Components.Resources;
using GameManager.Map;
using GameManager.Map.Generation;
using GameManager.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace GameManager.GameObjects.Components.Tilemaps
{
  public class Tilemap : Component
  {
    private const int CollisionRange = 2;
    private const int RenderRange = 20;
    private Tilemap.Tile oldPlayerTile;
    private readonly List<GameObject> resources = new List<GameObject>();
    private readonly List<DungeonEntrance> dungeonEntrances = new List<DungeonEntrance>();
    private readonly List<Vector2> savestoneLocations = new List<Vector2>();
    public const int Scale = 50;
    private static bool waterCollisionEnabled = true;
    private static bool isPlayerLevitating = false;

    public ReadOnlyCollection<DungeonEntrance> DungeonEntrances
    {
      get => this.dungeonEntrances.AsReadOnly();
    }

    public Random Random { get; private set; }

    public static Rect2 BuildingRect { get; private set; } 
            = new Rect2((Vector2) new Vector2Int(0, 0), (Vector2) new Vector2Int(30, 30));

    public Coordinate2DArray<Tilemap.Tile> Tiles { get; private set; }

    public static Tilemap Instance { get; private set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Seed { get; set; }

    public int HalfWidth => this.Width / 2;

    public int HalfHeight => this.Height / 2;

    public static bool WaterCollisionEnabled
    {
      set
      {
        if (Tilemap.Instance.oldPlayerTile != null)
          Tilemap.Instance.SetColliderAreaEnabled(Tilemap.Instance.oldPlayerTile.TilePos, false);
        Tilemap.waterCollisionEnabled = value;
      }
    }

    public static bool IsPlayerLevitating
    {
      get => Tilemap.isPlayerLevitating;
      set
      {
        if (Tilemap.Instance.oldPlayerTile != null)
          Tilemap.Instance.SetColliderAreaEnabled(Tilemap.Instance.oldPlayerTile.TilePos, false);
        Tilemap.waterCollisionEnabled = !value;
        Tilemap.isPlayerLevitating = value;
      }
    }

    public Tilemap()
    {
        Tilemap.Instance = this;
    }

    public Tilemap(GameObject gameObject) : this()
    {
      this.GameObject = gameObject;
      Tilemap.Instance = this;
    }

    public Tilemap.Tile GetTile(Vector2 worldPos)
    {
      Vector2Int vector2Int = Vector2Int.RoundToInt(worldPos / 50f);
      return this.IsOutOfTileBounds(vector2Int) ? (Tilemap.Tile) null : this.Tiles[vector2Int];
    }

    public Vector2Int GetTileOffsetPos(int offsetDir, Vector2Int tilePos)
    {
      switch (offsetDir)
      {
        case 0:
          --tilePos.Y;
          return tilePos;
        case 1:
          ++tilePos.X;
          return tilePos;
        case 2:
          ++tilePos.Y;
          return tilePos;
        case 3:
          --tilePos.X;
          return tilePos;
        default:
          throw new ArgumentOutOfRangeException(nameof (offsetDir), (object) offsetDir, "Only adjacent directions 0-3 are allowed as per Manhattan distance.");
      }
    }

    public bool IsOutOfTileBounds(Vector2Int tilePos)
    {
      return this.IsOutOfTileBounds(tilePos.X, tilePos.Y);
    }

    public bool IsOutOfTileBounds(int x, int y)
    {
      return x >= this.HalfWidth || y >= this.HalfHeight || x < -this.HalfWidth || y < -this.HalfHeight;
    }

    private List<Tilemap.Decoration> LoadBiomeDecorations(string biomeName)
    {
      List<Tilemap.Decoration> decorations = new List<Tilemap.Decoration>();
      int num = 0;
      do
        ;
      while (this.LoadDecoration(biomeName, num++, decorations));
      return decorations;
    }

        // Update the LoadDecoration method to use StringBuilder instead of DefaultInterpolatedStringHandler
        private bool LoadDecoration(string biomeName, int index, List<Tilemap.Decoration> decorations)
        {
            Texture2D texture = null;
            bool isRotatable = false;
            bool isBlocking = false;
            StringBuilder sb = new StringBuilder(20);
            sb.Append("Content\\Deco_").Append(biomeName).Append("_B_").Append(index).Append(".xnb");

            if (File.Exists(sb.ToString()))
            {
                ContentManager content = Glob.Content;
                sb.Clear().Append("Deco_").Append(biomeName).Append("_B_").Append(index);
                string stringAndClear = sb.ToString();
                texture = content.Load<Texture2D>(stringAndClear);
                isBlocking = true;
            }
            else
            {
                sb.Clear().Append("Content\\Deco_").Append(biomeName).Append("_N_").Append(index).Append(".xnb");
                if (File.Exists(sb.ToString()))
                {
					ContentManager content = Glob.Content;
					sb.Clear().Append("Deco_").Append(biomeName).Append("_N_").Append(index);
                    string stringAndClear = sb.ToString();
                    texture = content.Load<Texture2D>(stringAndClear);
                }
                else
                {
                    sb.Clear().Append("Content\\Deco_").Append(biomeName).Append("_B_").Append(index).Append("R.xnb");
                    if (File.Exists(sb.ToString()))
                    {
                        ContentManager content = Glob.Content;
                        sb.Clear().Append("Deco_").Append(biomeName).Append("_B_").Append(index).Append("R");
                        string stringAndClear = sb.ToString();
                        texture = content.Load<Texture2D>(stringAndClear);
                        isBlocking = true;
                        isRotatable = true;
                    }
                    else
                    {
                        sb.Clear().Append("Content\\Deco_").Append(biomeName).Append("_N_").Append(index).Append("R.xnb");
                        if (File.Exists(sb.ToString()))
                        {
                            ContentManager content = Glob.Content;
                            sb.Clear().Append("Deco_").Append(biomeName).Append("_N_").Append(index).Append("R");
                            string stringAndClear = sb.ToString();
                            texture = content.Load<Texture2D>(stringAndClear);
                            isRotatable = true;
                        }
                    }
                }
            }
            if (texture == null)
                return false;
            decorations.Add(new Tilemap.Decoration(texture, isRotatable, isBlocking));
            return true;
        }

    private void GenerateTiles()
    {
      UIWorldMap.Instance.GenerateNew(this.Width, this.Height);
      if (StateManager.Instance.CurrentState is OverworldState)
      {
        Texture2D texture1 = Glob.Content.Load<Texture2D>("Tile_SettlementFloor");
        Texture2D mountainTexture = Glob.Content.Load<Texture2D>("Tile_Mountain");
        Texture2D texture2 = Glob.Content.Load<Texture2D>("Tile_Water");
        Texture2D texture3 = Glob.Content.Load<Texture2D>("Tile_Tree");
        Texture2D texture4 = Glob.Content.Load<Texture2D>("Tile_ForestFloor");
        Texture2D texture2D = Glob.Content.Load<Texture2D>("DungeonEntrance");
        Texture2D texture5 = Glob.Content.Load<Texture2D>("Tile_PlainsFloor");
        Texture2D texture6 = Glob.Content.Load<Texture2D>("Tile_Sand");
        Texture2D texture7 = Glob.Content.Load<Texture2D>("Savestone");
        List<Tilemap.Decoration> decorations1 = this.LoadBiomeDecorations("Building");
        this.LoadBiomeDecorations("Settlement");
        List<Tilemap.Decoration> mountainDecorations = this.LoadBiomeDecorations("Mountain");
        List<Tilemap.Decoration> decorations2 = this.LoadBiomeDecorations("Beach");
        List<Tilemap.Decoration> decorations3 = this.LoadBiomeDecorations("Forest");
        List<Tilemap.Decoration> decorations4 = this.LoadBiomeDecorations("Fields");
        FastNoiseLite fastNoiseLite1 = new FastNoiseLite(this.Seed);
        fastNoiseLite1.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fastNoiseLite1.SetFrequency(0.01f);
        FastNoiseLite fastNoiseLite2 = new FastNoiseLite(this.Seed - 1);
        fastNoiseLite2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fastNoiseLite2.SetFrequency(0.005f);
        FastNoiseLite fastNoiseLite3 = new FastNoiseLite(this.Seed - 2);
        fastNoiseLite3.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fastNoiseLite3.SetFrequency(0.01f);
        FastNoiseLite settlementNoiseMap = new FastNoiseLite(this.Seed - 3);
        settlementNoiseMap.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        settlementNoiseMap.SetFrequency(0.005f);
        FastNoiseLite fastNoiseLite4 = new FastNoiseLite(this.Seed - 4);
        fastNoiseLite4.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fastNoiseLite4.SetFrequency(0.005f);
        for (int x = -this.HalfWidth; x < this.HalfWidth; ++x)
        {
          for (int y = -this.HalfHeight; y < this.HalfHeight; ++y)
          {
            Vector2Int vector2Int1 = new Vector2Int(x * 50, y * 50);
            TileType type = TileType.Terrain;
            GameObject gameObject = (GameObject) null;
            GameObject floorObj = (GameObject) null;
            Collider collider = (Collider) null;
            bool isBlocking = false;
            bool flag1 = x > this.HalfWidth - 105 || x < -this.HalfWidth + 105 || y > this.HalfWidth - 105 || y < -this.HalfWidth + 105;
            if (!flag1 && (double) settlementNoiseMap.GetNoise((float) x, (float) y) > 0.85000002384185791)
            {
              Color saddleBrown = Color.SaddleBrown;
              floorObj = this.CreateTileObject(vector2Int1, texture1, TileShadowType.None, saddleBrown);
              bool flag2 = false;

              if (this.Random.Next(0, 10) == 0 
                                && !this.HasBuildingBlockerInRange(vector2Int1 / 50f, 5, settlementNoiseMap, 0.85f))
              {
                gameObject = this.InstantiateRandomDecoration
                                    (ref collider, ref type, decorations1, vector2Int1, 
                                    out isBlocking, saddleBrown, new TileShadowType?(TileShadowType.Wall));

                collider = gameObject.AddComponent<Collider>();
                collider.SizeOverride = new Rectangle(0, 0, 60, 60);
                type = TileType.Building;
                flag2 = true;
                for (int index1 = -2; index1 <= 2; ++index1)
                {
                  for (int index2 = -2; index2 <= 2; ++index2)
                  {
                    if (Math.Abs(index1) + Math.Abs(index2) <= 3)
                    {
                      Vector2Int vector2Int2 = new Vector2Int(
                          vector2Int1.X / 50 + index1, vector2Int1.Y / 50 + index2);

                      if (!this.IsOutOfTileBounds(vector2Int2) && this.Tiles[vector2Int2] != null)
                        this.SetTileShadowAndMinimap(vector2Int2 * 50, TileShadowType.Object, 
                            saddleBrown, minimapShadowType: new TileShadowType?(TileShadowType.Wall));
                    }
                  }
                }
              }
              if (type != TileType.Building)
              {
                for (int index3 = -2; index3 <= 2; ++index3)
                {
                  for (int index4 = -2; index4 <= 2; ++index4)
                  {
                    if (Math.Abs(index3) + Math.Abs(index4) <= 3)
                    {
                      Vector2Int vector2Int3 = new Vector2Int(vector2Int1.X / 50
                          + index3, vector2Int1.Y / 50 + index4);
                      if (!this.IsOutOfTileBounds(vector2Int3))
                      {
                        Tilemap.Tile tile = this.Tiles[vector2Int3];
                        if ((tile != null ? (tile.Type != TileType.Building ? 1 : 0) : 1) == 0)
                        {
                          flag2 = true;
                          this.SetTileShadowAndMinimap(vector2Int1, TileShadowType.Object, 
                              saddleBrown, minimapShadowType: new TileShadowType?(TileShadowType.Wall));
                        }
                      }
                    }
                  }
                }
              }
              if (!flag2)
              {
                if ((double) settlementNoiseMap.GetNoise((float) x, (float) y) > 0.85500001907348633
                                    && this.Random.Next(0, 50) == 0)
                {
                  bool flag3 = false;
                  for (int index = 0; index < this.savestoneLocations.Count; ++index)
                  {
                    if ((double) Vector2.Distance((Vector2) vector2Int1, this.savestoneLocations[index]) <= 5000.0)
                    {
                      flag3 = true;
                      break;
                    }
                  }
                  if (!flag3)
                  {
                    gameObject = this.CreateTileObject(vector2Int1, texture7, TileShadowType.Wall, 
                        Color.Gold, minimapShadowType: new TileShadowType?(TileShadowType.None));
                    gameObject.AddComponent<Savestone>();
                    this.savestoneLocations.Add(gameObject.Transform.Position);
                    type = TileType.Savestone;
                  }
                }
                else if (this.Random.Next(0, 70) == 0)
                  MatoranBuilder.Instance.NewMatoran((Vector2) vector2Int1);
              }
            }
            else if (flag1 || (double) fastNoiseLite2.GetNoise((float) x, (float) y) > 0.30000001192092896)
            {
              if ((x > this.HalfWidth - 100 || x < -this.HalfWidth + 100 
                                || y > this.HalfWidth - 100 ? 1 : (y < -this.HalfWidth + 100 ? 1 : 0)) != 0 
                                || (double) fastNoiseLite2.GetNoise((float) x, (float) y) > 0.40000000596046448)
              {
                Color lightBlue = Color.LightBlue;
                floorObj = this.CreateTileObject(vector2Int1, texture2, TileShadowType.None, lightBlue);
                collider = floorObj.AddComponent<Collider>();
                type = TileType.Water;
              }
              else if ((double) fastNoiseLite1.GetNoise((float) x, (float) y) > 0.30000001192092896)
              {
                CreateMountainTerrain(vector2Int1, mountainTexture, texture2D, mountainDecorations,
                    ref gameObject, ref floorObj, ref collider, ref type, ref isBlocking);
              }
              else
              {
                Color sandyBrown = Color.SandyBrown;
                floorObj = this.CreateTileObject(vector2Int1, texture6, TileShadowType.None, sandyBrown);
                if (!this.RandomlyGenerateDungeonEntrance(vector2Int1, texture2D, ref gameObject, ref type)
                                    && this.Random.Next(0, 12) == 0)
                  gameObject = this.InstantiateRandomDecoration(ref collider, ref type, decorations2, 
                      vector2Int1, out isBlocking, sandyBrown);
              }
            }
            else if ((double) fastNoiseLite1.GetNoise((float) x, (float) y) > 0.30000001192092896)
              CreateMountainTerrain(vector2Int1, mountainTexture, texture2D, mountainDecorations, 
                  ref gameObject, ref floorObj, ref collider, ref type, ref isBlocking);
            else if ((double) fastNoiseLite4.GetNoise((float) x, (float) y) > 0.40000000596046448)
            {
              Color minimapColor = new Color(250, 213, 165);
              floorObj = this.CreateTileObject(vector2Int1, texture6, TileShadowType.None, minimapColor);
              if (!this.RandomlyGenerateDungeonEntrance(vector2Int1, texture2D, ref gameObject, ref type)
                                && this.Random.Next(0, 200) == 0)
                gameObject = this.InstantiateRandomDecoration(ref collider, ref type, decorations4, 
                    vector2Int1, out isBlocking, minimapColor);
            }
            else if ((double) fastNoiseLite3.GetNoise((float) x, (float) y) > -0.40000000596046448)
            {
              Color darkGreen = Color.DarkGreen;
              int maxValue = (int) Tools.ConvertNumberRange(fastNoiseLite3.GetNoise((float) x, (float) y),
                  -0.4f, 0.5f, 40f, 5f);
              if (this.Random.Next(0, maxValue) == 0)
              {
                gameObject = this.CreateTileObject(vector2Int1, texture3, TileShadowType.Object,
                    darkGreen, minimapShadowType: new TileShadowType?(TileShadowType.Wall));
                collider = gameObject.AddComponent<Collider>();
                gameObject.AddComponent<Tree>();
                collider.SizeOverride = new Rectangle(0, 0, 16, 16);
                type = TileType.Tree;
                this.resources.Add(gameObject);
              }
              else if (!this.RandomlyGenerateDungeonEntrance(vector2Int1, texture2D, ref gameObject, ref type)
                                && this.Random.Next(0, maxValue / 2) == 0)
                gameObject = this.InstantiateRandomDecoration(ref collider, ref type, decorations3,
                    vector2Int1, out isBlocking, darkGreen);
              floorObj = this.CreateTileObject(vector2Int1, texture4, TileShadowType.None, darkGreen,
                  gameObject == null);
            }
            else
            {
              Color greenYellow = Color.GreenYellow;
              floorObj = this.CreateTileObject(vector2Int1, texture5, TileShadowType.None, greenYellow);
              if (!this.RandomlyGenerateDungeonEntrance(vector2Int1, texture2D, ref gameObject, ref type) && this.Random.Next(0, 6) == 0)
                gameObject = this.InstantiateRandomDecoration(ref collider, ref type, decorations4, vector2Int1, out isBlocking, greenYellow);
            }
            this.Tiles[x, y] = new Tilemap.Tile(vector2Int1, type, gameObject, floorObj, collider, isBlocking);
          }
        }
      }
      else
      {
        Color brown = Color.Brown;
        Texture2D texture8 = Glob.Content.Load<Texture2D>("Tile_DungeonWall");
        Texture2D texture9 = Glob.Content.Load<Texture2D>("Tile_DungeonFloor");
        List<Tilemap.Decoration> decorations = this.LoadBiomeDecorations("Dungeon");
        Rect2 tilemapRect = new Rect2((Vector2) new Vector2Int(0, -6), (Vector2) new Vector2Int(50, 44));
        Vector2Int exteriorDoorPos = new Vector2Int((int) tilemapRect.Center.X, (int) tilemapRect.Max.Y);
        (List<Rect2> rect2List, List<Vector2Int> doorPositions) = DungeonGenerator.Generate(tilemapRect, exteriorDoorPos, new int?(this.Seed));
        Rect2 rect2 = new Rect2((Vector2) new Vector2Int(0, this.HalfHeight - 3 - 2), (Vector2) new Vector2Int(4, 8));
        for (int x = -this.HalfWidth; x < this.HalfWidth; ++x)
        {
          for (int y = -this.HalfHeight; y < this.HalfHeight; ++y)
          {
            Vector2Int vector2Int4 = new Vector2Int(x * 50, y * 50);
            TileType type = TileType.Terrain;
            GameObject gameObject = (GameObject) null;
            Collider collider = (Collider) null;
            bool isBlocking = false;
            if (x == this.HalfWidth - 1 || y == -this.HalfWidth || tilemapRect.IsOnEdge(x, y) || this.IsOnRectEdge(x, y, rect2List) || rect2.IsOnEdge(x, y))
            {
              Vector2Int vector2Int5 = new Vector2Int(x, y);
              if ((x == this.HalfWidth - 1 || x == -this.HalfWidth || y == -this.HalfWidth || y == exteriorDoorPos.Y) && vector2Int5 != exteriorDoorPos || !doorPositions.Contains(vector2Int5))
              {
                gameObject = this.CreateTileObject(vector2Int4, texture8, TileShadowType.Wall, brown);
                collider = gameObject.AddComponent<Collider>();
              }
            }
            if (gameObject == null && this.Random.Next(0, 5) == 0)
              gameObject = this.InstantiateRandomDecoration(ref collider, ref type, decorations, vector2Int4, out isBlocking, brown);
            GameObject tileObject = this.CreateTileObject(vector2Int4, texture9, TileShadowType.None, brown, gameObject == null);
            this.Tiles[x, y] = new Tilemap.Tile(vector2Int4, type, gameObject, tileObject, collider, isBlocking);
          }
        }
        for (int index5 = 0; index5 < rect2List.Count; ++index5)
        {
          if (IsValidRoom(rect2List[index5]))
          {
            int num = this.Random.Next(0, GetMaxEnemyAmount((int) rect2List[index5].Size.LengthSquared()) + 1);
            for (int index6 = 0; index6 < num; ++index6)
            {
              Vector2 pos = this.GetRandomPosInRect(rect2List[index5], 1f) * 50f;
              if (!this.IsOutOfTileBounds(Vector2Int.RoundToInt(pos / 50f)))
                EnemyBuilder.Instance.NewEnemy(pos);
            }
          }
        }
      }

      static bool IsValidRoom(Rect2 room)
      {
        return (double) room.Size.X >= 1.0 && (double) room.Size.Y >= 1.0;
      }

      static int GetMaxEnemyAmount(int roomArea) => roomArea >= 6 ? Math.Max(1, roomArea / 30) : 0;

      void CreateMountainTerrain(
        Vector2Int pos,
        Texture2D mountainTexture,
        Texture2D dungeonEntranceTexture,
        List<Tilemap.Decoration> mountainDecorations,
        ref GameObject gameObject,
        ref GameObject floorObj,
        ref Collider collider,
        ref TileType type,
        ref bool isDecoBlocking)
      {
        Color gray = Color.Gray;
        if (this.Random.Next(0, 3) == 0)
        {
          gameObject = this.CreateTileObject(pos, mountainTexture, TileShadowType.Wall, gray);
          collider = gameObject.AddComponent<Collider>();
        }
        else
        {
          floorObj = this.CreateTileObject(pos, mountainTexture, TileShadowType.None, gray);
          if (this.RandomlyGenerateDungeonEntrance(pos, dungeonEntranceTexture, ref gameObject, ref type) || this.Random.Next(0, 8) != 0)
            return;
          gameObject = this.InstantiateRandomDecoration(ref collider, ref type, mountainDecorations, pos, out isDecoBlocking, gray);
        }
      }
    }

    private void CountEnemies()
    {
      if (!(StateManager.Instance.CurrentState is DungeonState currentState))
        return;
      Vector2 origin = new Vector2(0.0f, 22f) * 50f;
      for (int index = 0; index < Enemy.Enemies.Count; ++index)
      {
        if (Pathfinder.Search(origin, Enemy.Enemies[index].Transform.Position) != null)
          ++currentState.RemainingEnemyCount;
      }
    }

    private bool HasBuildingBlockerInRange(
      Vector2Int origin,
      int range,
      FastNoiseLite settlementNoiseMap,
      float settlementNoiseLevel)
    {
      for (int index1 = -range; index1 <= range; ++index1)
      {
        for (int index2 = -range; index2 <= range; ++index2)
        {
          Vector2Int vector2Int = new Vector2Int(origin.X + index1, origin.Y + index2);
          if (this.IsOutOfTileBounds(vector2Int))
            return true;
          TileType? type = this.Tiles[vector2Int]?.Type;
          if (type.HasValue)
          {
            switch (type.GetValueOrDefault())
            {
              case TileType.Building:
                return true;
              case TileType.Savestone:
                return true;
            }
          }
          if (Math.Abs(index1) + Math.Abs(index2) <= 3 && (double) settlementNoiseMap.GetNoise((float) vector2Int.X, (float) vector2Int.Y) <= (double) settlementNoiseLevel)
            return true;
        }
      }
      return false;
    }

    private GameObject InstantiateRandomDecoration(
      ref Collider collider,
      ref TileType type,
      List<Tilemap.Decoration> decorations,
      Vector2Int pos,
      out bool isBlocking,
      Color minimapColor,
      TileShadowType? minimapShadowType = null)
    {
      Tilemap.Decoration randomElement = decorations.GetRandomElement<Tilemap.Decoration>(this.Random);
      GameObject tileObject = this.CreateTileObject(pos, randomElement.Texture, TileShadowType.Object, minimapColor, minimapShadowType: minimapShadowType);
      if (randomElement.IsRotatable)
        tileObject.Transform.Rotation = this.Random.NextFloat(-3.14159274f, 3.14159274f);
      tileObject.Transform.Scale /= 1.5f;
      SpriteRenderer component = tileObject.GetComponent<SpriteRenderer>();
      component.FlipX = this.Random.Next(0, 2) == 0;

      if (!randomElement.IsBlocking)
        component.SpriteBatchOverride = Game1.FloorDecoSpriteBatch;

      isBlocking = randomElement.IsBlocking;
      switch (component.Sprite.Name)
      {
        case "Deco_Dungeon_N_5R":
          LightEmitter lightEmitter1 = tileObject.AddComponent<LightEmitter>();
          lightEmitter1.Color = new LightColor(1f, 0.843137264f, 0.0f);
          lightEmitter1.Intensity = 0.75f;
          lightEmitter1.Range = 5;
          break;
        case "Deco_Mountain_N_4":
          LightEmitter lightEmitter2 = tileObject.AddComponent<LightEmitter>();
          lightEmitter2.Color = new LightColor(0.0f, 0.0f, (float) byte.MaxValue);
          lightEmitter2.Intensity = 1f;
          lightEmitter2.Range = 2;
          break;
        case "Deco_Beach_B_7":
          collider = tileObject.AddComponent<Collider>();
          tileObject.AddComponent<Harakeke>();
          this.resources.Add(tileObject);
          type = TileType.Resource;
          break;
        case "Deco_Mountain_B_7":
          collider = tileObject.AddComponent<Collider>();
          tileObject.AddComponent<Ore>();
          this.resources.Add(tileObject);
          type = TileType.Resource;
          break;
        case "Deco_Forest_B_8":
          collider = tileObject.AddComponent<Collider>();
          tileObject.AddComponent<Bamboo>();
          this.resources.Add(tileObject);
          type = TileType.Resource;
          break;
      }
      return tileObject;
    }

    private bool RandomlyGenerateDungeonEntrance(
      Vector2Int pos,
      Texture2D texture,
      ref GameObject gameObject,
      ref TileType type)
    {
      if (this.Random.Next(0, 1000) != 0)
        return false;
      type = TileType.DungeonEntrance;
      gameObject = this.CreateTileObject(pos, texture, TileShadowType.Wall, Color.Cyan, minimapShadowType: new TileShadowType?(TileShadowType.None));
      DungeonEntrance dungeonEntrance = gameObject.AddComponent<DungeonEntrance>();
      dungeonEntrance.IsEnabled = false;
      dungeonEntrance.Seed = this.Random.Next();
      this.dungeonEntrances.Add(dungeonEntrance);
      return true;
    }

    public GameObject CreateTileObject(
      Vector2Int pos,
      Texture2D texture,
      TileShadowType shadowType,
      Color minimapColor,
      bool addToMinimap = true,
      TileShadowType? minimapShadowType = null)
    {
      if (texture == null)
        return (GameObject) null;
      GameObject tileObject = new GameObject(false);
      tileObject.Transform.Position = (Vector2) pos;
      tileObject.Transform.Scale = 3.125f;
      SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
      spriteRenderer.Sprite = texture;
      if (shadowType == TileShadowType.None)
        spriteRenderer.SpriteBatchOverride = Game1.FloorSpriteBatch;
      this.SetTileShadowAndMinimap(pos, shadowType, minimapColor, addToMinimap, minimapShadowType);
      return tileObject;
    }

    private void SetTileShadowAndMinimap(
      Vector2Int pos,
      TileShadowType shadowType,
      Color minimapColor,
      bool addToMinimap = true,
      TileShadowType? minimapShadowType = null)
    {
      switch (shadowType)
      {
        case TileShadowType.Object:
          EdgeShadowTilemap.Instance.SetTileObject(pos / 50f, true);
          break;
        case TileShadowType.Wall:
          EdgeShadowTilemap.Instance.SetTileWall(pos / 50f, true);
          break;
      }
      if (!addToMinimap)
        return;
            UIWorldMap.Instance.SetTile(pos / 50f, minimapColor, minimapShadowType.HasValue 
                ? minimapShadowType.Value : shadowType);
    }

    private Vector2 GetRandomPosInRect(Rect2 rect, float buffer = 0.0f)
    {
      return new Vector2(this.Random.NextFloat(rect.Min.X + buffer, rect.Max.X - buffer), this.Random.NextFloat(rect.Min.Y + buffer, rect.Max.Y - buffer));
    }

    private bool IsOnRectEdge(int x, int y, List<Rect2> rectangles)
    {
      for (int index = 0; index < rectangles.Count; ++index)
      {
        if (rectangles[index].IsOnEdge(x, y))
          return true;
      }
      return false;
    }

    public void Start()
    {
      this.Random = new Random(this.Seed);
      this.Tiles = new Coordinate2DArray<Tilemap.Tile>(this.Width, this.Height);
      this.GenerateTiles();
      this.GeneratePathfindingGrid();
      this.CountEnemies();
    }

    public void Update()
    {
        Microsoft.Xna.Framework.Vector2 pos = new Microsoft.Xna.Framework.Vector2((float)Math.Round(Player.Instance.Transform.Position.X / 50f),
            (float)Math.Round(Player.Instance.Transform.Position.Y / 50f));       

      if (this.IsOutOfTileBounds((int) pos.X, (int) pos.Y))
        return;
      this.UpdatePlayerTile(this.Tiles[pos]);
    }

    private void UpdatePlayerTile(Tilemap.Tile newPlayerTile)
    {
      if (newPlayerTile != this.oldPlayerTile)
      {
        if (this.oldPlayerTile != null)
        {
          this.SetColliderAreaEnabled(this.oldPlayerTile.TilePos, false);
          this.SetRenderAreaEnabled(this.oldPlayerTile.TilePos, false);
          this.oldPlayerTile.GameObject?.GetComponent<DungeonEntrance>()?.SetEnabled(false);
          this.oldPlayerTile.GameObject?.GetComponent<Savestone>()?.SetEnabled(false);
        }
        this.SetColliderAreaEnabled(newPlayerTile.TilePos, true);
        this.SetRenderAreaEnabled(newPlayerTile.TilePos, true);
        newPlayerTile.GameObject?.GetComponent<DungeonEntrance>()?.SetEnabled(true);
        newPlayerTile.GameObject?.GetComponent<Savestone>()?.SetEnabled(true);
      }
      this.oldPlayerTile = newPlayerTile;
    }

    private void SetColliderAreaEnabled(Vector2Int tilePos, bool value)
    {
      for (int index1 = -2; index1 <= 2; ++index1)
      {
        for (int index2 = -2; index2 <= 2; ++index2)
        {
          Vector2Int vector2Int = new Vector2Int(tilePos.X + index1, tilePos.Y + index2);
          if (!this.IsOutOfTileBounds(vector2Int.X, vector2Int.Y))
          {
            Tilemap.Tile tile = this.Tiles[vector2Int.X, vector2Int.Y];
            if ((Tilemap.waterCollisionEnabled || tile.Type != TileType.Water) && (!Tilemap.isPlayerLevitating || tile.Type != TileType.Tree && tile.Type != TileType.Resource))
              tile.SetCollisionEnabled(value);
          }
        }
      }
    }

    private void SetRenderAreaEnabled(Vector2Int tilePos, bool value)
    {
      for (int index1 = -20; index1 <= 20; ++index1)
      {
        for (int index2 = -20; index2 <= 20; ++index2)
        {
          Vector2Int vector2Int = new Vector2Int(tilePos.X + index1, tilePos.Y + index2);
          if (!this.IsOutOfTileBounds(vector2Int.X, vector2Int.Y))
            this.Tiles[vector2Int.X, vector2Int.Y].SetRenderingEnabled(value);
        }
      }
    }

    public void GeneratePathfindingGrid()
    {
      for (int x = -this.HalfWidth; x < this.HalfWidth; ++x)
      {
        for (int y = -this.HalfHeight; y < this.HalfHeight; ++y)
          this.Tiles[x, y].UpdateWalkableNeighbours();
      }
    }

    public class Tile
    {
      private readonly GameObject floorObj;
      private readonly List<VisibilityProvider> visibleBy = new List<VisibilityProvider>();
      private LightColor light = LightingTilemap.Instance.Darkness;
      private readonly List<LightEmitter> lightSources = new List<LightEmitter>();
      private List<Tilemap.Tile> walkableNeighbours;

      public TileType Type { get; set; }

      public GameObject GameObject { get; set; }

      public bool IsSlow { get; }

      public Collider Collider { private get; set; }

      public Vector2Int Position { get; }

      public Vector2Int TilePos => this.Position / 50f;

      public bool IsWall => this.Collider != null;

      public bool IsTraversable => this.Collider == null || this.Type == TileType.Water;

      public bool IsOpaque
      {
        get
        {
          return this.IsWall && this.Type != TileType.Water && this.Type != TileType.Building && this.Type != TileType.Resource;
        }
      }

      public int Cost => !this.IsSlow ? 1 : 4;

      public ReadOnlyCollection<Tilemap.Tile> WalkableNeighbours
      {
        get => this.walkableNeighbours.AsReadOnly();
      }

      public ReadOnlyCollection<VisibilityProvider> VisibleBy => this.visibleBy.AsReadOnly();

      public Tile(
        Vector2Int position,
        TileType type,
        GameObject gameObject,
        GameObject floorObj,
        Collider collider,
        bool isSlow)
      {
        this.Type = type;
        this.Position = position;
        this.GameObject = gameObject;
        this.floorObj = floorObj;
        this.Collider = collider;
        this.IsSlow = isSlow;
      }

      public void SetCollisionEnabled(bool value)
      {
        if (this.Collider == null)
          return;
        this.Collider.IsEnabled = value;
      }

      public void SetRenderingEnabled(bool value)
      {
        this.GameObject?.SetActive(value);
        this.floorObj?.SetActive(value);
      }

      public void UpdateWalkableNeighbours()
      {
        this.walkableNeighbours = new List<Tilemap.Tile>();
        for (int offsetDir = 0; offsetDir < 4; ++offsetDir)
        {
          Vector2Int tileOffsetPos = Tilemap.Instance.GetTileOffsetPos(offsetDir, this.TilePos);
          if (!Tilemap.Instance.IsOutOfTileBounds(tileOffsetPos.X, tileOffsetPos.Y))
          {
            Tilemap.Tile tile = Tilemap.Instance.Tiles[tileOffsetPos];
            if (!tile.IsWall)
              this.walkableNeighbours.Add(tile);
          }
        }
      }

      public void ChangeLighting(
        bool gainedVision,
        LightColor gainedLight,
        LightEmitter lightSource = null)
      {
        if (gainedVision)
        {
          if (lightSource != null)
          {
            if (!this.lightSources.Contains(lightSource))
            {
              this.lightSources.Add(lightSource);
              this.light += gainedLight;
            }
          }
          else
            this.light += gainedLight;
        }
        else if (lightSource != null)
        {
          if (this.lightSources.Remove(lightSource))
            this.light -= gainedLight;
        }
        else
          this.light -= gainedLight;
        LightingTilemap instance = LightingTilemap.Instance;
        Vector2Int tilePos = this.TilePos;
        int x = tilePos.X;
        tilePos = this.TilePos;
        int y = tilePos.Y;
        Color light = (Color) this.light;
        instance.SetTileLighting(x, y, light);
      }

      public void ChangeVision(bool gainedVision, VisibilityProvider provider)
      {
        if (gainedVision)
        {
          if (this.visibleBy.Count == 0 && !this.IsOpaque)
            VisionTilemap.Instance.SetTileVisible(this.TilePos, true);
          if (this.visibleBy.Contains(provider))
            return;
          this.visibleBy.Add(provider);
        }
        else
        {
          this.visibleBy.Remove(provider);
          if (this.visibleBy.Count != 0 && !this.IsOpaque)
            return;
          VisionTilemap.Instance.SetTileVisible(this.TilePos, false);
        }
      }
    }

    public struct Decoration(Texture2D texture, bool isRotatable, bool isBlocking)
    {
      public Texture2D Texture { get; } = texture;

      public bool IsRotatable { get; } = isRotatable;

      public bool IsBlocking { get; } = isBlocking;
    }
  }
}
