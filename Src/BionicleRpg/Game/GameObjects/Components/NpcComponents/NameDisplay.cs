
// Type: GameManager.GameObjects.Components.NpcComponents.NameDisplay
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.GameObjects.Components.NpcComponents
{
  public class NameDisplay : Component
  {
    private Npc npc;
    private Vector2 position;

    public NameDisplay()
    { }
        
    public NameDisplay(GameObject gameObject) : this()
    {       
    }
        
    public void Awake() => this.npc = this.GetComponent<Npc>();

    public void Draw(SpriteBatch spriteBatch)
    {
      Color color = this.npc is Enemy ? Color.Red : Color.White;

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont,
          this.npc.Name, this.position + Vector2.One, Color.Black, 0.0f,
          Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont,
          this.npc.Name, this.position, color, 0.0f, Vector2.Zero, 0.5f, 
          SpriteEffects.None, 1f);
    }

    public void LastUpdate()
    {
      this.position = new Vector2((float) (
          (double) this.npc.Transform.Position.X - (double) Player.Instance.Transform.Position.X 
          + (double) Game1.ScreenSize.X / 2.0 -
          (double) UIManager.Instance.UIFont.MeasureString(this.npc.Name).X / 2.0 * 0.5), 
          (float) ((double) this.npc.Transform.Position.Y - (double) Player.Instance.Transform.Position.Y 
          + (double) Game1.ScreenSize.Y / 2.0 - 60.0));
    }
  }
}
