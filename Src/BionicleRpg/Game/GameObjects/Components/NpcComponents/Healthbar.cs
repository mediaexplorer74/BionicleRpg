
// Type: GameManager.GameObjects.Components.NpcComponents.Healthbar
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.GameObjects.Components.NpcComponents
{
  public class Healthbar : Component
  {
    private Npc npc;
    private Health health;
    private Texture2D healthFrameSprite;
    private Texture2D healthSprite;
    private Vector2 position;
    private Rectangle rct;

    public Healthbar() 
    {
        
    }

    public Healthbar(GameObject gameObject) : this()
    {   
        
    }

    public void Awake()
    {
      this.npc = this.GetComponent<Npc>();
      this.health = this.GetComponent<Health>();
      this.healthFrameSprite = Glob.Content.Load<Texture2D>("Healthbar0");
      this.healthSprite = Glob.Content.Load<Texture2D>("Healthbar10");
      this.rct = new Rectangle(0, 0, this.healthSprite.Width, this.healthSprite.Height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Color color = this.npc is Enemy ? Color.Red : Color.White;
      Game1.UISpriteBatch.Draw(this.healthFrameSprite, this.position, color);
      Game1.UISpriteBatch.Draw(this.healthSprite, this.position, new Rectangle?(this.rct), color);
    }

    public void LastUpdate()
    {
      this.position = new Vector2((float) ((double) this.npc.Transform.Position.X 
          - (double) Player.Instance.Transform.Position.X + (double) Game1.ScreenSize.X / 2.0) 
          - (float) (this.healthSprite.Width / 2), (float) ((double) this.npc.Transform.Position.Y 
          - (double) Player.Instance.Transform.Position.Y + (double) Game1.ScreenSize.Y / 2.0 - 40.0));
      this.rct.Width = (int) ((double) this.healthSprite.Width / (double) this.health.MaxHealth * (double) this.health.CurrentHealth);
    }
  }
}
