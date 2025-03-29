
// Type: GameManager.Map.Generation.DungeonGenerator
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager.Map.Generation
{
    
        
  public class DungeonGenerator
  {
    private const float minHallwayPercentage = 0.25f;
    private const int minHallwayWidth = 2;
    private const int maxHallwayWidth = 2;
    private const int minRoomArea = 5;
    private const int maxRoomArea = 100;
    private const float randomRoomAreaPercentage = 0.0f;
    private readonly Random random;
    private readonly List<Rect2> chunks = new List<Rect2>();
    private int hallwayArea;

    private DungeonGenerator(int? seed)
    {
      this.random = new Random(seed ?? Game1.Random.Next());
    }

    public static (List<Rect2> rooms, List<Vector2Int> doorPositions) Generate(
      Rect2 tilemapRect,
      Vector2Int exteriorDoorPos,
      int? seed = null)
    {
      DungeonGenerator dungeonGenerator = new DungeonGenerator(seed);
      dungeonGenerator.DesignateHallways(tilemapRect, (Vector2) exteriorDoorPos);
      dungeonGenerator.DesignateRooms();
      List<Vector2Int> vector2IntList = dungeonGenerator.DesignateDoors(tilemapRect);
      vector2IntList.Add(exteriorDoorPos);
      return (dungeonGenerator.chunks, vector2IntList);
    }

    private static int GetChunkArea(Rect2 rect) => (int) rect.Size.X * (int) rect.Size.Y;

    private void DesignateHallways(Rect2 tilemapRect, Vector2 exteriorDoorPos)
    {
      bool isHorizontal = this.random.Next(0, 2) == 0;
      int chunkArea = DungeonGenerator.GetChunkArea(tilemapRect);
      this.NewHallway(isHorizontal, tilemapRect, new Vector2?(exteriorDoorPos));
      while ((double) this.hallwayArea / (double) chunkArea < 0.25)
      {
        isHorizontal = !isHorizontal;
        this.NewHallway(isHorizontal);
      }
    }

    private void NewHallway(bool isHorizontal)
    {
      int num = int.MinValue;
      Rect2 chunk = new Rect2();
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        int chunkArea = DungeonGenerator.GetChunkArea(this.chunks[index]);
        if (chunkArea > num)
        {
          num = chunkArea;
          chunk = this.chunks[index];
        }
      }
      this.chunks.Remove(chunk);
      this.NewHallway(isHorizontal, chunk);
    }

    private void NewHallway(bool isHorizontal, Rect2 chunk, Vector2? doorPos = null)
    {
      this.SplitChunk(this.random.Next(2, 3), !isHorizontal, chunk, doorPos);
    }

    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }


    private void SplitChunk(int hallwayWidth, bool isHorizontal, Rect2 chunk, Vector2? doorPos)
    {
      if (isHorizontal)
      {
        this.hallwayArea += (int) chunk.Size.X * hallwayWidth;
        float num1;
        if (!doorPos.HasValue)
        {
          float num2 = chunk.Min.Y + (float) hallwayWidth + (float) this.random.Next(1, (int) chunk.Size.Y - hallwayWidth);
          float min = (float) ((double) chunk.Min.Y + (double) hallwayWidth + 5.0);
          float max = (float) ((double) chunk.Max.Y - (double) hallwayWidth - 5.0);
          num1 = (double) min <= (double) max ? Clamp(num2, min, max) : min;
        }
        else
          num1 = (double) doorPos.Value.Y >= (double) chunk.Min.Y + (double) hallwayWidth ? ((double) doorPos.Value.Y <= (double) chunk.Max.Y - (double) hallwayWidth ? doorPos.Value.Y : chunk.Max.Y - (float) hallwayWidth) : chunk.Min.Y + (float) hallwayWidth;
        float chunkStart1 = num1 + (float) hallwayWidth;
        float y1 = chunk.Max.Y;
        if ((double) chunkStart1 <= (double) y1)
          this.GenerateBoundsY(chunk.Center.X, chunk.Size.X, chunkStart1, y1);
        float chunkStart2 = num1 - (float) hallwayWidth;
        float y2 = chunk.Min.Y;
        if ((double) chunkStart2 < (double) y2)
          return;
        this.GenerateBoundsY(chunk.Center.X, chunk.Size.X, chunkStart2, y2);
      }
      else
      {
        this.hallwayArea += (int) chunk.Size.Y * hallwayWidth;
        float num3;
        if (!doorPos.HasValue)
        {
          float num4 = chunk.Min.X + (float) hallwayWidth + (float) this.random.Next(1, (int) chunk.Size.X - hallwayWidth);
          float min = (float) ((double) chunk.Min.X + (double) hallwayWidth + 5.0);
          float max = (float) ((double) chunk.Max.X - (double) hallwayWidth - 5.0);
          num3 = (double) min <= (double) max ? Clamp(num4, min, max) : min;
        }
        else
          num3 = (double) doorPos.Value.X >= (double) chunk.Min.X + (double) hallwayWidth ? ((double) doorPos.Value.X <= (double) chunk.Max.X - (double) hallwayWidth ? doorPos.Value.X : chunk.Max.X - (float) hallwayWidth) : chunk.Min.X + (float) hallwayWidth;
        float chunkStart3 = num3 + (float) hallwayWidth;
        float x1 = chunk.Max.X;
        if ((double) chunkStart3 <= (double) x1)
          this.GenerateBoundsX(chunk.Center.Y, chunk.Size.Y, chunkStart3, x1);
        float chunkStart4 = num3 - (float) hallwayWidth;
        float x2 = chunk.Min.X;
        if ((double) chunkStart4 < (double) x2)
          return;
        this.GenerateBoundsX(chunk.Center.Y, chunk.Size.Y, chunkStart4, x2);
      }
    }

    private void GenerateBoundsY(float centerX, float sizeX, float chunkStart, float chunkEnd)
    {
      this.chunks.Add(new Rect2(new Vector2(centerX, (float) (((double) chunkStart + (double) chunkEnd) / 2.0)), new Vector2(sizeX, Math.Abs(chunkStart - chunkEnd))));
    }

    private void GenerateBoundsX(float centerY, float sizeY, float chunkStart, float chunkEnd)
    {
      this.chunks.Add(new Rect2(new Vector2((float) (((double) chunkStart + (double) chunkEnd) / 2.0), centerY), new Vector2(Math.Abs(chunkStart - chunkEnd), sizeY)));
    }

    private void DesignateRooms()
    {
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        while (DungeonGenerator.GetChunkArea(this.chunks[index]) > 2 && ((double) this.chunks[index].Size.X >= (double) this.chunks[index].Size.Y * 3.0 || (double) this.chunks[index].Size.Y >= (double) this.chunks[index].Size.X * 3.0 || DungeonGenerator.GetChunkArea(this.chunks[index]) > this.random.Next(5, 101) || (double) this.random.NextFloat(0.0f, 1f) < 0.0))
        {
          Rect2 rect2;
          if ((double) this.chunks[index].Size.X > (double) this.chunks[index].Size.Y)
          {
            //Vector2 center1;
            //ref Vector2 local1 = ref center1;
            double x1 = (double) Math.Round(this.chunks[index].Center.X + this.chunks[index].Extents.X / 2f);
            Rect2 chunk1 = this.chunks[index];
            double y1 = (double) chunk1.Center.Y;
            Vector2 center1 = new Vector2((float)x1, (float)y1);
            //Vector2 local1 = new Vector2((float) x1, (float) y1);
            //Vector2 center2;
            //ref Vector2 local2 = ref center2;
            chunk1 = this.chunks[index];
            double x2 = (double) chunk1.Center.X;
            chunk1 = this.chunks[index];
            double num = (double) chunk1.Extents.X / 2.0;
            double x3 = (double) Math.Round((float) (x2 - num));
            Rect2 chunk2 = this.chunks[index];
            double y2 = (double) chunk2.Center.Y;
            Vector2 center2 = new Vector2((float)x3, (float)y2);
            //Vector2 local2 = new Vector2((float) x3, (float) y2);
            //Vector2 size;
            //ref Vector2 local3 = ref size;
            chunk2 = this.chunks[index];
            double x4 = (double) Math.Round(chunk2.Extents.X);
            chunk2 = this.chunks[index];
            double y3 = (double) chunk2.Size.Y;
            Vector2 size = new Vector2((float)x4, (float)y3);
            //Vector2 local3 = new Vector2((float) x4, (float) y3);
            rect2 = new Rect2(center1, size);
            this.chunks[index] = new Rect2(center2, size);
          }
          else
          {
            Vector2 center3 = new Vector2(this.chunks[index].Center.X, (float)Math.Round(this.chunks[index].Center.Y + this.chunks[index].Extents.Y / 2f));
            Vector2 center4 = new Vector2(this.chunks[index].Center.X, (float)Math.Round(this.chunks[index].Center.Y - this.chunks[index].Extents.Y / 2f));
            Vector2 size = new Vector2(this.chunks[index].Size.X, (float)Math.Round(this.chunks[index].Extents.Y));
            rect2 = new Rect2(center3, size);
            this.chunks[index] = new Rect2(center4, size);
          }
          this.chunks.Add(rect2);
        }
      }
    }

    private List<Vector2Int> DesignateDoors(Rect2 tilemapRect)
    {
      HashSet<Vector2Int> vector2IntSet = new HashSet<Vector2Int>();
      List<Vector2Int> vector2IntList = new List<Vector2Int>();
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        int num1 = 0;
        int num2 = this.random.Next(1, 5);
        Rect2 chunk = this.chunks[index];
        if ((double) chunk.Min.X > (double) tilemapRect.Min.X)
        {
          chunk = this.chunks[index];
          int x = (int) chunk.Min.X;
          chunk = this.chunks[index];
          int y1 = (int) chunk.Center.Y;
          Vector2Int edge = new Vector2Int(x, y1);
          chunk = this.chunks[index];
          double y2 = (double) (int) chunk.Size.Y;
          HashSet<Vector2Int> visitedEdges = vector2IntSet;
          List<Vector2Int> doorPositions = vector2IntList;
          this.PlaceDoor(edge, (float) y2, false, visitedEdges, doorPositions);
          if (++num1 >= num2)
            continue;
        }
        chunk = this.chunks[index];
        if ((double) chunk.Max.X < (double) tilemapRect.Max.X)
        {
          chunk = this.chunks[index];
          int x = (int) chunk.Max.X;
          chunk = this.chunks[index];
          int y3 = (int) chunk.Center.Y;
          Vector2Int edge = new Vector2Int(x, y3);
          chunk = this.chunks[index];
          double y4 = (double) (int) chunk.Size.Y;
          HashSet<Vector2Int> visitedEdges = vector2IntSet;
          List<Vector2Int> doorPositions = vector2IntList;
          this.PlaceDoor(edge, (float) y4, false, visitedEdges, doorPositions);
          if (++num1 >= num2)
            continue;
        }
        chunk = this.chunks[index];
        if ((double) chunk.Min.Y > (double) tilemapRect.Min.Y)
        {
          chunk = this.chunks[index];
          int x1 = (int) chunk.Center.X;
          chunk = this.chunks[index];
          int y = (int) chunk.Min.Y;
          Vector2Int edge = new Vector2Int(x1, y);
          chunk = this.chunks[index];
          double x2 = (double) (int) chunk.Size.X;
          HashSet<Vector2Int> visitedEdges = vector2IntSet;
          List<Vector2Int> doorPositions = vector2IntList;
          this.PlaceDoor(edge, (float) x2, true, visitedEdges, doorPositions);
          int num3;
          if ((num3 = num1 + 1) >= num2)
            continue;
        }
        chunk = this.chunks[index];
        if ((double) chunk.Max.Y < (double) tilemapRect.Max.Y)
        {
          chunk = this.chunks[index];
          int x3 = (int) chunk.Center.X;
          chunk = this.chunks[index];
          int y = (int) chunk.Max.Y;
          Vector2Int edge = new Vector2Int(x3, y);
          chunk = this.chunks[index];
          double x4 = (double) (int) chunk.Size.X;
          HashSet<Vector2Int> visitedEdges = vector2IntSet;
          List<Vector2Int> doorPositions = vector2IntList;
          this.PlaceDoor(edge, (float) x4, true, visitedEdges, doorPositions);
        }
      }
      return vector2IntList;
    }

    private void PlaceDoor(
      Vector2Int edge,
      float size,
      bool isHorizontal,
      HashSet<Vector2Int> visitedEdges,
      List<Vector2Int> doorPositions)
    {
      if (visitedEdges.Contains(edge))
        return;
      visitedEdges.Add(edge);
      int minValue = (int) (-(double) size / 2.0 + 1.0);
      int maxValue = (int) ((double) size / 2.0 - 1.0);
      if (minValue > maxValue)
        return;
      int num = this.random.Next(minValue, maxValue);
      Vector2Int vector2Int = edge;
      if (isHorizontal)
        vector2Int.X += num;
      else
        vector2Int.Y += num;
      doorPositions.Add(vector2Int);
    }

    public struct GenerationInfo(List<Rect2> rooms, List<Vector2Int> doorPositions)
    {
      public List<Rect2> Rooms { get; } = rooms;

      public List<Vector2Int> DoorPositions { get; } = doorPositions;
    }
  }
}
