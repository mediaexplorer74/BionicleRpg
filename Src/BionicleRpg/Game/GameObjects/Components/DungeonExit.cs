
// Type: GameManager.GameObjects.Components.DungeonExit

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.States;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.GameObjects.Components
{
  public class DungeonExit : Building
  {
    private static readonly string indicatorText = "Press F / Tap L.S. to exit";

    public int Seed { get; set; }

    public DungeonExit() 
    { }

    public DungeonExit(GameObject gameObject) : this() 
    { }

    public void Start()
    {
        this.Transform.Scale = /*0.75f*/4f; // enlarged for better ui :)       
        this.SetEnabled(false);       
    }

    public void Exit()
    {
        StateManager.Instance.RemoveScreen();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Vector2 position = new Vector2((float) ((double) this.Transform.Position.X 
          - (double) Player.Instance.Transform.Position.X 
          + (double) Game1.ScreenSize.X / 2.0 
          - (double) UIManager.Instance.UIFont.MeasureString(
              DungeonExit.indicatorText).X / 2.0 * 0.5), 
          (float) ((double) this.Transform.Position.Y - 
          (double) Player.Instance.Transform.Position.Y
          + (double) Game1.ScreenSize.Y / 2.0 - 50.0));

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont,
          DungeonExit.indicatorText, position + Vector2.One, Color.Black, 0.0f, 
          Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, 
          DungeonExit.indicatorText, position, Color.White, 0.0f, 
          Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
    }

    public void SetEnabled(bool value)
    {
        this.IsEnabled = value;
    }
  }
}
