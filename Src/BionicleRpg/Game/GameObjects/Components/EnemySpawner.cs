
// Type: GameManager.GameObjects.Components.EnemySpawner
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Builders;
using GameManager.DataTypes;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using System;


namespace GameManager.GameObjects.Components
{
  public class EnemySpawner : Component
  {
    private const float maxSpawnInterval = 1500f;
    private const float minSpawnInterval = 150f;
    private float spawnInterval = 500f;
    private Vector2 lastSpawnPlayerPos;
    private Vector2 oldPlayerPos;

    public static EnemySpawner Instance { get; } = new EnemySpawner();

    public EnemySpawner() 
    { }

    public EnemySpawner(GameObject gameObject) : this()
    { }

    public void Update()
    {
      Vector2? moveDir = new Vector2?(this.oldPlayerPos - Player.Instance.Transform.Position);
      this.oldPlayerPos = Player.Instance.Transform.Position;
      if ((double) Vector2.Distance(this.lastSpawnPlayerPos, Player.Instance.Transform.Position) < (double) this.spawnInterval)
        return;
      this.lastSpawnPlayerPos = Player.Instance.Transform.Position;
      this.spawnInterval = Game1.Random.NextFloat(150f, 1500f);
      if ((double) moveDir.Value.LengthSquared() == 0.0)
        moveDir = new Vector2?();
      Vector2? enemySpawnPos = this.GetEnemySpawnPos(moveDir);
      if (!enemySpawnPos.HasValue)
        return;
      Vector2Int origin = Vector2Int.RoundToInt(enemySpawnPos.Value / 50f);
      if (Tilemap.Instance.IsOutOfTileBounds(origin.X, origin.Y))
        return;
      if (Tilemap.Instance.Tiles[origin.X, origin.Y].IsWall)
      {
        Vector2Int? nearestFreePos = EnemySpawner.GetNearestFreePos(origin);
        if (!nearestFreePos.HasValue)
          return;
        origin = nearestFreePos.Value;
      }
      EnemyBuilder.Instance.NewEnemy((Vector2) (origin * 50));
    }

    private static Vector2Int? GetNearestFreePos(Vector2Int origin)
    {
      for (int index1 = -2; index1 <= 2; ++index1)
      {
        for (int index2 = -2; index2 <= 2; ++index2)
        {
          Vector2Int vector2Int = new Vector2Int(origin.X + index1, origin.Y + index2);
          if (!Tilemap.Instance.IsOutOfTileBounds(vector2Int.X, vector2Int.Y) && !Tilemap.Instance.Tiles[vector2Int.X, vector2Int.Y].IsWall)
            return new Vector2Int?(vector2Int);
        }
      }
      return new Vector2Int?();
    }

    private Vector2? GetEnemySpawnPos(Vector2? moveDir = null)
    {
      Vector2 position = Player.Instance.Transform.Position;
      float num1 = (float) ((double) Game1.ScreenSize.X / 2.0 + 15.0);
      float num2 = (float) ((double) Game1.ScreenSize.Y / 2.0 + 15.0);
      if (moveDir.HasValue)
      {
        if ((double) Math.Abs(moveDir.Value.X) > (double) Math.Abs(moveDir.Value.Y))
        {
          if (Game1.Random.Next(0, 2) != 0)
            return new Vector2?();
          if ((double) moveDir.Value.X > 0.0)
            position.X -= num1;
          else
            position.X += num1;
          position.Y += Game1.Random.NextFloat(-num2, num2);
          return new Vector2?(position);
        }
        if ((double) Math.Abs(moveDir.Value.Y) > (double) Math.Abs(moveDir.Value.X))
        {
          if (Game1.Random.Next(0, 2) != 0)
            return new Vector2?();
          if ((double) moveDir.Value.Y > 0.0)
            position.Y -= num2;
          else
            position.Y += num2;
          position.X += Game1.Random.NextFloat(-num1, num1);
          return new Vector2?(position);
        }
      }
      return new Vector2?(this.GetRandomEdgePos(position, num1, num2));
    }

    private Vector2 GetRandomEdgePos(Vector2 spawnPos, float xMax, float yMax)
    {
      if (Game1.Random.Next(0, 2) == 0)
      {
        spawnPos.X += Game1.Random.Next(0, 2) == 0 ? xMax : -xMax;
        spawnPos.Y += Game1.Random.NextFloat(-yMax, yMax);
      }
      else
      {
        spawnPos.Y += Game1.Random.Next(0, 2) == 0 ? yMax : -yMax;
        spawnPos.X += Game1.Random.NextFloat(-xMax, xMax);
      }
      return spawnPos;
    }
  }
}
