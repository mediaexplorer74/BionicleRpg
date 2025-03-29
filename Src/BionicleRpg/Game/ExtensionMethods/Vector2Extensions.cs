
// Type: GameManager.Vector2Extensions
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public static class Vector2Extensions
  {
    public static Vector2 Normalized(this Vector2 v)
    {
      v.Normalize();
      return v;
    }

    public static float GetAngle(this Vector2 v) => (float)Math.Atan2(v.Y, v.X);
  }
}
