
// Type: GameManager.GameObjects.UI.Image
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.GameObjects.UI
{
  public class Image : UIComponent
  {
    public Color Color { get; set; }

    public Image(string spriteName, Vector2 position, float scale)
    {
      this.spriteName = spriteName;
      this.position = position;
      this.scale = scale;
      this.Color = Color.White;
    }

    public override void Awake()
    {
      //RND
      this.Sprite = Glob.Content.Load<Texture2D>(this.spriteName);
      this.origin = new Vector2((float) (this.Sprite.Width / 2), (float) (this.Sprite.Height / 2));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Sprite, this.position, new Rectangle?(), 
          this.Color, 0.0f, this.origin, this.scale, SpriteEffects.None, 0.0f);
    }
  }
}
