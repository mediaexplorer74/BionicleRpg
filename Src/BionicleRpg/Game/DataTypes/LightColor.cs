
// Type: GameManager.DataTypes.LightColor
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace GameManager.DataTypes
{
  public struct LightColor(float r, float g, float b)
  {
    public float R { get; set; } = r;

    public float G { get; set; } = g;

    public float B { get; set; } = b;

    public static LightColor operator +(LightColor a, LightColor b)
    {
      return new LightColor(a.R + b.R, a.G + b.G, a.B + b.B);
    }

    public static LightColor operator -(LightColor a, LightColor b)
    {
      return new LightColor(a.R - b.R, a.G - b.G, a.B - b.B);
    }

    public static LightColor operator *(LightColor a, float b)
    {
      return new LightColor(a.R * b, a.G * b, a.B * b);
    }

    public static implicit operator Color(LightColor obj) => new Color(obj.R, obj.G, obj.B, 1f);
  }
}
