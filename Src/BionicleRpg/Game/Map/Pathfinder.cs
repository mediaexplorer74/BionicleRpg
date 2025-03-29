
// Type: GameManager.Map.Pathfinder
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#nullable disable
namespace GameManager.Map
{
  public static class Pathfinder
  {
    private const int maxCost = 500000;

    public static ReadOnlyCollection<Vector2Int> Search(Vector2 origin, Vector2 goal)
    {
      return Pathfinder.Search(Vector2Int.RoundToInt(origin) / 50f, Vector2Int.RoundToInt(goal) / 50f);
    }

    public static ReadOnlyCollection<Vector2Int> Search(Vector2Int origin, Vector2Int goal)
    {
      PriorityQueue<Vector2Int, int> priorityQueue = new PriorityQueue<Vector2Int, int>((IEnumerable<(Vector2Int, int)>) new (Vector2Int, int)[1]
      {
        (origin, 0)
      });
      Dictionary<Vector2Int, Vector2Int> dictionary1 = new Dictionary<Vector2Int, Vector2Int>();
      Dictionary<Vector2Int, int> dictionary2 = new Dictionary<Vector2Int, int>();
      dictionary1[origin] = origin;
      dictionary2[origin] = 0;
      bool flag = false;
      while (priorityQueue.Count > 0)
      {
        Vector2Int pos = priorityQueue.Dequeue();
        if (pos.Equals(goal))
        {
          flag = true;
          break;
        }
        Tilemap.Tile tile = Tilemap.Instance.Tiles[pos];

        foreach (Tilemap.Tile walkableNeighbour in tile.WalkableNeighbours)
        {
           int num = 510000; //RnD

           try
           {
               num = dictionary2[tile.TilePos] + walkableNeighbour.Cost;
           }
           catch (Exception ex)
           {
              Debug.WriteLine("[ex] PathFinder error: " + ex.Message + " [" + ex.StackTrace + "]");
           }

          if (num <= 500000)
          {
            if (!dictionary2.ContainsKey(walkableNeighbour.TilePos) || num < dictionary2[walkableNeighbour.TilePos])
            {
              dictionary2[walkableNeighbour.TilePos] = num;
              int priority = num + Pathfinder.Heuristic(walkableNeighbour.TilePos, goal);
              priorityQueue.Enqueue(walkableNeighbour.TilePos, priority);
              dictionary1[walkableNeighbour.TilePos] = pos;
            }
          }
          else
            break;
        }
      }
      if (!flag)
        return (ReadOnlyCollection<Vector2Int>) null;
      List<Vector2Int> vector2IntList = new List<Vector2Int>();
      for (Vector2Int key = goal; key != origin; key = dictionary1[key])
        vector2IntList.Add(key * 50);
      vector2IntList.Reverse();
      return vector2IntList.AsReadOnly();
    }

    public static ReadOnlyCollection<Vector2Int> Search(Vector2 origin, float fleeDist)
    {
      return Pathfinder.Search(Vector2Int.RoundToInt(origin) / 50f, fleeDist);
    }

    public static ReadOnlyCollection<Vector2Int> Search(Vector2Int origin, float fleeDist)
    {
      PriorityQueue<Vector2Int, int> priorityQueue = new PriorityQueue<Vector2Int, int>((IEnumerable<(Vector2Int, int)>) new (Vector2Int, int)[1]
      {
        (origin, 0)
      });
      Dictionary<Vector2Int, Vector2Int> dictionary1 = new Dictionary<Vector2Int, Vector2Int>();
      Dictionary<Vector2Int, int> dictionary2 = new Dictionary<Vector2Int, int>();
      dictionary1[origin] = origin;
      dictionary2[origin] = 0;
      Vector2Int vector2Int = Vector2Int.Zero;
      bool flag = false;
      while (priorityQueue.Count > 0)
      {
        Vector2Int pos = priorityQueue.Dequeue();
        Tilemap.Tile tile = Tilemap.Instance.Tiles[pos];
        if (tile.VisibleBy.Count == 0 || (double) Vector2.Distance((Vector2) origin, (Vector2) pos) >= (double) fleeDist)
        {
          vector2Int = pos;
          flag = true;
          break;
        }
        foreach (Tilemap.Tile walkableNeighbour in tile.WalkableNeighbours)
        {
          int num = dictionary2[tile.TilePos] + walkableNeighbour.Cost;
          if (!dictionary2.ContainsKey(walkableNeighbour.TilePos) || num < dictionary2[walkableNeighbour.TilePos])
          {
            dictionary2[walkableNeighbour.TilePos] = num;
            int priority = num - Pathfinder.Heuristic(walkableNeighbour.TilePos, origin);
            priorityQueue.Enqueue(walkableNeighbour.TilePos, priority);
            dictionary1[walkableNeighbour.TilePos] = pos;
          }
        }
      }
      if (!flag)
        return (ReadOnlyCollection<Vector2Int>) null;
      List<Vector2Int> vector2IntList = new List<Vector2Int>();
      for (Vector2Int key = vector2Int; key != origin; key = dictionary1[key])
        vector2IntList.Add(key * 50);
      vector2IntList.Reverse();
      return vector2IntList.AsReadOnly();
    }

    private static int Heuristic(Vector2Int a, Vector2Int b)
    {
      return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
  }
}
