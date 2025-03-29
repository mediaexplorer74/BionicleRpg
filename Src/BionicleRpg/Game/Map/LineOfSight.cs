
// Type: GameManager.Map.LineOfSight
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Lighting;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace GameManager.Map
{
  public static class LineOfSight
  {
    private static readonly int[,] multipliers = new int[4, 8]
    {
      {
        1,
        0,
        0,
        -1,
        -1,
        0,
        0,
        1
      },
      {
        0,
        1,
        -1,
        0,
        0,
        -1,
        1,
        0
      },
      {
        0,
        1,
        1,
        0,
        0,
        -1,
        -1,
        0
      },
      {
        1,
        0,
        0,
        1,
        -1,
        0,
        0,
        -1
      }
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetVisible(int x, int y, bool gainVision, VisibilityProvider provider)
    {
      Tilemap.Instance.Tiles[x, y].ChangeVision(gainVision, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ChangeLighting(
      int x,
      int y,
      bool gainVision,
      LightColor gainedLight,
      LightEmitter lightSource)
    {
      Tilemap.Instance.Tiles[x, y].ChangeLighting(gainVision, gainedLight, lightSource);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsOpaque(int x, int y) => Tilemap.Instance.Tiles[x, y].IsOpaque;

    private static void Shadowcast(
      int x,
      int y,
      int radius,
      int row,
      float slopeStart,
      float slopeEnd,
      int xx,
      int xy,
      int yx,
      int yy,
      bool gainVision,
      bool allowedEdge,
      VisibilityProvider provider)
    {
      if ((double) slopeStart < (double) slopeEnd)
        return;
      float num1 = slopeStart;
      for (int index = row; index <= radius; ++index)
      {
        bool flag = false;
        int num2 = -index;
        int num3 = -index;
        for (; num2 <= 0; ++num2)
        {
          float num4 = (float) (((double) num2 + 0.5) / ((double) num3 - 0.5));
          if ((double) slopeStart >= (double) num4)
          {
            float slopeEnd1 = (float) (((double) num2 - 0.5) / ((double) num3 + 0.5));
            if ((double) slopeEnd <= (double) slopeEnd1)
            {
              int num5 = num2 * xx + num3 * xy;
              int num6 = num2 * yx + num3 * yy;
              if ((num5 > -Tilemap.Instance.HalfWidth || Math.Abs(num5) <= x) && (num6 > -Tilemap.Instance.HalfHeight || Math.Abs(num6) <= y))
              {
                int x1 = x + num5;
                int y1 = y + num6;
                if (!Tilemap.Instance.IsOutOfTileBounds(x1, y1))
                {
                  int num7 = radius * radius;
                  int num8 = num2 * num2 + num3 * num3;
                  if ((allowedEdge || num2 < 0 && num2 != -index) && !LineOfSight.IsOpaque(x1, y1) && num8 < num7)
                    LineOfSight.SetVisible(x1, y1, gainVision, provider);
                  if (flag)
                  {
                    if (LineOfSight.IsOpaque(x1, y1))
                    {
                      num1 = num4;
                    }
                    else
                    {
                      flag = false;
                      slopeStart = num1;
                    }
                  }
                  else if (LineOfSight.IsOpaque(x1, y1))
                  {
                    flag = true;
                    num1 = num4;
                    LineOfSight.Shadowcast(x, y, radius, index + 1, slopeStart, slopeEnd1, xx, xy, yx, yy, gainVision, allowedEdge, provider);
                  }
                }
              }
            }
            else
              break;
          }
        }
        if (flag)
          break;
      }
    }

    private static void Shadowcast(
      int x,
      int y,
      int radius,
      int row,
      float slopeStart,
      float slopeEnd,
      int xx,
      int xy,
      int yx,
      int yy,
      bool gainVision,
      bool allowedEdge,
      LightColor light,
      LightEmitter lightSource,
      float minAngle,
      float maxAngle)
    {
      if ((double) slopeStart < (double) slopeEnd)
        return;
      float num1 = slopeStart;
      for (int index = row; index <= radius; ++index)
      {
        bool flag = false;
        int num2 = -index;
        int num3 = -index;
        for (; num2 <= 0; ++num2)
        {
          float num4 = (float) (((double) num2 + 0.5) / ((double) num3 - 0.5));
          if ((double) slopeStart >= (double) num4)
          {
            float slopeEnd1 = (float) (((double) num2 - 0.5) / ((double) num3 + 0.5));
            if ((double) slopeEnd <= (double) slopeEnd1)
            {
              int num5 = num2 * xx + num3 * xy;
              int num6 = num2 * yx + num3 * yy;
              if ((num5 > -Tilemap.Instance.HalfWidth || Math.Abs(num5) <= x) && (num6 > -Tilemap.Instance.HalfHeight || Math.Abs(num6) <= y))
              {
                int x1 = x + num5;
                int y1 = y + num6;
                if (!Tilemap.Instance.IsOutOfTileBounds(x1, y1))
                {
                  float angle = (new Vector2((float) x, (float) y) - new Vector2((float) x1, (float) y1)).GetAngle();
                  if ((double) angle >= (double) minAngle && (double) angle <= (double) maxAngle)
                  {
                    int num7 = radius * radius;
                    int num8 = num2 * num2 + num3 * num3;
                    if ((allowedEdge || num2 < 0 && num2 != -index) && !LineOfSight.IsOpaque(x1, y1) && num8 < num7)
                    {
                      float num9 = (float) (x1 - x);
                      float num10 = (float) (y1 - y);
                      float num11 = (float) (1.0 - (double) Math.Sqrt((float) ((double) num9 * (double) num9 + (double) num10 * (double) num10)) / ((double) radius - 1.0));
                      if ((double) minAngle != -3.1415927410125732 || (double) maxAngle != 3.1415927410125732)
                        num11 = num11 * Math.Min(1f, Math.Abs(angle - minAngle) * 3f) * Math.Min(1f, Math.Abs(angle - maxAngle) * 3f);
                      if ((double) num11 < 0.0)
                        num11 = 0.0f;
                      LineOfSight.ChangeLighting(x1, y1, gainVision, light * num11, lightSource);
                    }
                    if (flag)
                    {
                      if (LineOfSight.IsOpaque(x1, y1))
                      {
                        num1 = num4;
                      }
                      else
                      {
                        flag = false;
                        slopeStart = num1;
                      }
                    }
                    else if (LineOfSight.IsOpaque(x1, y1))
                    {
                      flag = true;
                      num1 = num4;
                      LineOfSight.Shadowcast(x, y, radius, index + 1, slopeStart, slopeEnd1, xx, xy, yx, yy, gainVision, allowedEdge, light, lightSource, minAngle, maxAngle);
                    }
                  }
                }
              }
            }
            else
              break;
          }
        }
        if (flag)
          break;
      }
    }

    public static void UpdateLOS(
      int x,
      int y,
      int radius,
      bool gainVision,
      VisibilityProvider provider)
    {
      LineOfSight.SetVisible(x, y, gainVision, provider);
      for (int index = 0; index < 8; ++index)
        LineOfSight.Shadowcast(x, y, radius, 1, 1f, 0.0f, LineOfSight.multipliers[0, index], LineOfSight.multipliers[1, index], LineOfSight.multipliers[2, index], LineOfSight.multipliers[3, index], gainVision, index % 2 == 0, provider);
    }

    public static void UpdateLOS(
      int x,
      int y,
      int radius,
      bool gainVision,
      LightColor light,
      LightEmitter lightSource,
      float minAngle = -180f,
      float maxAngle = 180f)
    {
      LineOfSight.ChangeLighting(x, y, gainVision, light, lightSource);
      for (int index = 0; index < 8; ++index)
        LineOfSight.Shadowcast(x, y, radius + 1, 1, 1f, 0.0f, LineOfSight.multipliers[0, index], LineOfSight.multipliers[1, index], LineOfSight.multipliers[2, index], LineOfSight.multipliers[3, index], gainVision, index % 2 == 0, light, lightSource, MathHelper.ToRadians(minAngle), MathHelper.ToRadians(maxAngle));
    }
  }
}
