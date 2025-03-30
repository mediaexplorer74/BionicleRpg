
// Type: GameManager.UI.Text
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.UI
{
  public class Text : UIComponent
  {
    private Color color;
    private TextAlignment textAlignment;

    public string TextString { get; set; }

    public Text(
      string text,
      Vector2 position,
      Color color,
      float scale,
      TextAlignment textAlignment)
    {
      this.TextString = text;
      this.position = position;
      this.color = color;
      this.scale = scale;
      this.textAlignment = textAlignment;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this.TextString == null)
        return;
      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, this.TextString, 
          this.position, this.color, this.scale, this.textAlignment);
    }
  }
}
