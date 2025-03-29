
// Type: GameManager.Map.UIWorldMap
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace GameManager.Map
{
    public class UIWorldMap
    {
        public static UIWorldMap Instance { get; } = new UIWorldMap();

        public Texture2D Texture { get; set; }

        public void GenerateNew(int width, int height)
        {
            this.Texture = new Texture2D(Glob.GraphicsDevice, width, height);
        }

        public void SetTile(Vector2Int tilePos, Color color, TileShadowType shadowType)
        {
            float num1;
            switch (shadowType)
            {
                case TileShadowType.None:
                    num1 = 1f;
                    break;
                case TileShadowType.Object:
                    num1 = 0.875f;
                    break;
                case TileShadowType.Wall:
                    num1 = 0.75f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shadowType), shadowType, null);
            }
            float num2 = num1;
            color *= num2;
            color.A = byte.MaxValue;
            this.SetPixel(tilePos.X + Tilemap.Instance.HalfWidth, tilePos.Y + Tilemap.Instance.HalfHeight, color);
        }

        private void SetPixel(int x, int y, Color color)
        {
            this.Texture.SetData<Color>(0, new Rectangle?(new Rectangle(x, y, 1, 1)), new Color[1]
            {
            color
            }, 0, 1);
        }
    }
}
