
// Type: GameManager.DataTypes.Coordinate2DArray`1
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.DataTypes
{
  public class Coordinate2DArray<T>
  {
    private T[,] coordinates;

    public T this[int x, int y]
    {
      get => this.coordinates[x + Tilemap.Instance.HalfWidth, y + Tilemap.Instance.HalfHeight];
      set
      {
        this.coordinates[x + Tilemap.Instance.HalfWidth, y + Tilemap.Instance.HalfHeight] = value;
      }
    }

    public T this[Vector2Int pos]
    {
      get => this[pos.X, pos.Y];
      set => this[pos.X, pos.Y] = value;
    }

    public T this[Vector2 pos]
    {
      get => this[(int) pos.X, (int) pos.Y];
      set => this[(int) pos.X, (int) pos.Y] = value;
    }

    public Coordinate2DArray(int width, int height) => this.coordinates = new T[width, height];
  }
}
