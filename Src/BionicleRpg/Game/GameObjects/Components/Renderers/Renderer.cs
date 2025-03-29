
// Type: GameManager.GameObjects.Components.Renderers.Renderer
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.GameObjects.Components.Renderers
{
  public abstract class Renderer : Component
  {
    public Vector2 origin;
   
    protected Renderer()
    {
            
    }

    protected Renderer(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public Color Color { get; set; } = Color.White;

    public SortingLayer SortingLayer { private get; set; }

    public int OrderInLayer { private get; set; }

    public SpriteBatch SpriteBatchOverride { private get; set; }

    protected virtual float LayerDepth => 0.0f;

    public Vector2 PosOffset { get; set; } = Vector2.Zero;

    protected void DrawTexture(
      SpriteBatch spriteBatch,
      Texture2D texture,
      bool flipX,
      float scaleMultiplier = 1f)
    {
      if (!this.IsEnabled)
        return;
      if (this.SpriteBatchOverride != null)
        spriteBatch = this.SpriteBatchOverride;

      spriteBatch.Draw(texture, this.Transform.Position 
          - Player.Instance.Transform.Position + Game1.ScreenSize / 2f + this.PosOffset,
          new Rectangle?(), this.Color, this.Transform.Rotation, this.origin, 
          this.Transform.Scale * scaleMultiplier, flipX
          ? SpriteEffects.FlipHorizontally 
          : SpriteEffects.None,
          this.LayerDepth);
    }
  }
}
