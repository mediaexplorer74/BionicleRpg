
// Type: GameManager.GameObjects.Components.Renderers.GenericRenderer
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;


namespace GameManager.GameObjects.Components.Renderers
{
  public class GenericRenderer : Renderer
  {
    private IRenderable renderable;

    public GenericRenderer() 
    { }

    public GenericRenderer(GameObject gameObject) : this() { }

    public IRenderable Renderable
    {
      set
      {
        this.IsEnabled = value != null;
        this.renderable = value;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      this.DrawTexture(spriteBatch, this.renderable.Texture, false);
    }
  }
}
