
// Type: GameManager.Tools
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class Tools
  {
    public static float Rotation(float degrees)
    {
      return (float) ((double) degrees * 3.1415927410125732 / 180.0);
    }

    public static Vector2 GetVectorFromAngle(float angle)
    {
      return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    public static float Lerp(float a, float b, float t)
    {
      if ((double) t > 1.0)
        t = 1f;
      else if ((double) t < 0.0)
        t = 0.0f;
      return a + (b - a) * t;
    }

    public static float SmoothStep(float a, float b, float t)
    {
      if ((double) t < 0.0)
        t = 0.0f;
      if ((double) t > 1.0)
        t = 1f;
      t = (float) (-2.0 * (double) t * (double) t * (double) t + 3.0 * (double) t * (double) t);
      return (float) ((double) b * (double) t + (double) a * (1.0 - (double) t));
    }

    public static float ConvertNumberRange(
      float value,
      float oldMin,
      float oldMax,
      float newMin,
      float newMax)
    {
      if ((double) value > (double) oldMax)
        value = oldMax;
      if ((double) value < (double) oldMin)
        value = oldMin;
      return (float) (((double) value - (double) oldMin) * ((double) newMax - (double) newMin) / ((double) oldMax - (double) oldMin)) + newMin;
    }
  }
}
