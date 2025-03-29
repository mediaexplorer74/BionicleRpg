
// Type: GameManager.GameObjects.Transform
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.GameObjects
{
  public class Transform
  {
    public Vector2 Position { get; set; }

    public float Scale { get; set; } = 1f;

    public float Rotation { get; set; }

    public float WorldDirection { get; set; }

    public void Translate(Vector2 translation) => this.Position += translation;
  }
}
