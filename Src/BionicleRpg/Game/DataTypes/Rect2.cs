
// Type: GameManager.DataTypes.Rect2
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;


namespace GameManager.DataTypes
{
    public struct Rect2(Vector2 center, Vector2 size)
    {
        public Vector2 Center { get; set; } = center;

        public Vector2 Size { get; set; } = size;

        public readonly Vector2 Extents => this.Size / 2f;

        public readonly Vector2 Max => this.Center + this.Extents;

        public readonly Vector2 Min => this.Center - this.Extents;

        public float Top => this.Min.Y;

        public float Bottom => this.Max.Y;

        public float Left => this.Min.X;

        public float Right => this.Max.X;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Center: ({0}, {1}), Extents: ({2}, {3})",
                this.Center.X, this.Center.Y, this.Extents.X, this.Extents.Y);
        }

        public readonly bool Contains(int x, int y)
        {
            return (double)this.Min.X <= (double)x && (double)x < (double)this.Max.X && (double)this.Min.Y <= (double)y && (double)y < (double)this.Max.Y;
        }

        public readonly bool IsOnEdge(int x, int y)
        {
            return ((double)x == (double)this.Min.X || (double)x == (double)this.Max.X) && (double)y >= (double)this.Min.Y && (double)y <= (double)this.Max.Y || ((double)y == (double)this.Min.Y || (double)y == (double)this.Max.Y) && (double)x >= (double)this.Min.X && (double)x <= (double)this.Max.X;
        }

        public static implicit operator Rectangle(Rect2 obj)
        {
            Vector2 vector2_1 = new Vector2((float)Math.Round(obj.Center.X - obj.Size.X / 2f), (float)Math.Round(obj.Center.Y - obj.Size.Y / 2f));
            Vector2 vector2_2 = new Vector2((float)Math.Round(obj.Size.X), (float)Math.Round(obj.Size.Y));
            return new Rectangle(new Point((int)vector2_1.X, (int)vector2_1.Y), new Point((int)vector2_2.X, (int)vector2_2.Y));
        }
    }
}
