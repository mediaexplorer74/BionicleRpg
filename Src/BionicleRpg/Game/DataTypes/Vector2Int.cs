
// Type: GameManager.DataTypes.Vector2Int
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


namespace GameManager.DataTypes
{
  [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
  public struct Vector2Int(int x, int y) : IEquatable<Vector2Int>
  {
    public int X { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] set; } = x;

    public int Y { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] set; } = y;

    public static Vector2Int Zero => new Vector2Int(0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
      return "{X:" + this.X.ToString() + " Y:" + this.Y.ToString() + "}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2Int other) => this.X == other.X && this.Y == other.Y;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(/*[NotNullWhen(true)]*/ object obj)
    {
      return obj is Vector2Int vector2Int && this == vector2Int;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => this.X * 49157 + this.Y * 98317;

    public static Vector2Int RoundToInt(Vector2 v)
    {
      return new Vector2Int((int) Math.Round(v.X), (int) Math.Round(v.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector2Int obj)
    {
      return new Vector2((float) obj.X, (float) obj.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Point(Vector2Int obj) => new Point(obj.X, obj.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator bool(Vector2Int? v) => v.HasValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator *(Vector2Int v, int multiplier)
    {
      return new Vector2Int(v.X * multiplier, v.Y * multiplier);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator /(Vector2Int v, float divider)
    {
      float num = 1f / divider;
      v.X = (int) ((double) v.X * (double) num);
      v.Y = (int) ((double) v.Y * (double) num);
      return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2Int a, Vector2Int b) => a.Equals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2Int a, Vector2Int b) => !a.Equals(b);
  }
}
