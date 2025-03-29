
// Type: GameManager.GameObjects.Components.Items.Masks.MaskEnergyBar
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class MaskEnergyBar : Component
  {
    private Player player;
    private Mask mask;
    private Texture2D energyFrameSprite;
    private Texture2D energySprite;
    private Vector2 position;
    private Rectangle rct;

        public MaskEnergyBar()
        {
        }

        public MaskEnergyBar(GameObject gameObject) : this()
        {
            this.GameObject = gameObject;           
        }

        public void Awake()
    {
      this.player = this.GetComponent<Player>();
      this.mask = Player.Instance.MaskComponent;
      this.energyFrameSprite = Glob.Content.Load<Texture2D>("MaskEnergyBar0");
      this.energySprite = Glob.Content.Load<Texture2D>("MaskEnergyBar10");
      this.rct = new Rectangle(0, 0, this.energySprite.Width, this.energySprite.Height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.mask == null || !this.mask.PowerActive && 
                (double) this.mask.MaskEnergy >= (double) this.mask.MaxMaskEnergy)
        return;

      Color color = (double) this.mask.MaskEnergy > (double) this.mask.MinMaskEnergy 
                ? Color.LimeGreen : Color.Yellow;

      Game1.UISpriteBatch.Draw(this.energyFrameSprite, this.position, color);

      Game1.UISpriteBatch.Draw(this.energySprite, 
          this.position, new Rectangle?(this.rct), color);
    }

    public void LastUpdate()
    {
      if (this.mask == null)
        return;
      this.position = new Vector2((float) ((double) this.player.Transform.Position.X - (double) Player.Instance.Transform.Position.X + (double) Game1.ScreenSize.X / 2.0) - (float) (this.energySprite.Width / 2), (float) ((double) this.player.Transform.Position.Y - (double) Player.Instance.Transform.Position.Y + (double) Player.Instance.GetToaOffset().Y + (double) Game1.ScreenSize.Y / 2.0 - 60.0));
      this.rct.Width = (int) ((double) this.energySprite.Width / (double) this.mask.MaxMaskEnergy * (double) this.mask.MaskEnergy);
    }
  }
}
