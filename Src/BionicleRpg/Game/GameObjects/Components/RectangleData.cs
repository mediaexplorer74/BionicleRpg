
// Type: GameManager.GameObjects.Components.RectangleData
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace GameManager.GameObjects.Components
{
  public class RectangleData
  {
    public Rectangle Rectangle { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public RectangleData(int x, int y)
    {
      this.Rectangle = new Rectangle();
      this.X = x;
      this.Y = y;
    }
  }
}
