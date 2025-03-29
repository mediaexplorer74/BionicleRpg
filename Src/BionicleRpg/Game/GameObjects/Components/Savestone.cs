
// Type: GameManager.GameObjects.Components.Savestone
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Savestone : Building
  {
    private static readonly string indicatorText = "Press F to save";

    public int Seed { get; set; }

    public Savestone() { }

    public Savestone(GameObject gameObject) : this()
    {
        
    }

    public void Start() => this.SetEnabled(false);

    public void Draw(SpriteBatch spriteBatch)
    {
      Vector2 position = new Vector2((float) ((double) this.Transform.Position.X
          - (double) Player.Instance.Transform.Position.X + (double) Game1.ScreenSize.X / 2.0
          - (double) UIManager.Instance.UIFont.MeasureString(Savestone.indicatorText).X / 2.0 * 0.5),
          (float) ((double) this.Transform.Position.Y - (double) Player.Instance.Transform.Position.Y
          + (double) Game1.ScreenSize.Y / 2.0 - 50.0));

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, Savestone.indicatorText,
          position + Vector2.One, Color.Black, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, Savestone.indicatorText, 
          position, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
    }

    public void SetEnabled(bool value) => this.IsEnabled = value;
  }
}
