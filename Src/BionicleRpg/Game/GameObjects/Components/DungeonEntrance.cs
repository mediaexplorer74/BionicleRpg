
// Type: GameManager.GameObjects.Components.DungeonEntrance
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.States;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.GameObjects.Components
{
  public class DungeonEntrance : Building
  {
    private static readonly string indicatorText = "Press F to enter";

    public int Seed { get; set; }

    public DungeonEntrance() 
    { }

    public DungeonEntrance(GameObject gameObject) : this() 
    { }

    public void Start() => this.Transform.Scale = 0.75f;

    public void Enter() => StateManager.Instance.AddScreen((IState) new DungeonState(this));

    public void Draw(SpriteBatch spriteBatch)
    {
      Vector2 position = new Vector2((float) ((double) this.Transform.Position.X 
          - (double) Player.Instance.Transform.Position.X + (double) Game1.ScreenSize.X / 2.0 
          - (double) UIManager.Instance.UIFont.MeasureString(DungeonEntrance.indicatorText).X / 2.0 * 0.5), 
          (float) ((double) this.Transform.Position.Y - (double) Player.Instance.Transform.Position.Y
          + (double) Game1.ScreenSize.Y / 2.0 - 50.0));

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont,
          DungeonEntrance.indicatorText, position + Vector2.One, Color.Black, 0.0f, 
          Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, 
          DungeonEntrance.indicatorText, position, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
    }

    public void SetEnabled(bool value) => this.IsEnabled = value;
  }
}
