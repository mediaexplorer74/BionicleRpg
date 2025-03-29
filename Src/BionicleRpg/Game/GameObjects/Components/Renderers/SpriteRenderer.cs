
// Type: GameManager.GameObjects.Components.Renderers.SpriteRenderer
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.GameObjects.Components.Renderers
{
  public class SpriteRenderer : Renderer
  {
    private Texture2D sprite;
    private float? layerPosOffset;

    public SpriteRenderer()
    {
    }

    public SpriteRenderer(GameObject gameObject) : this()
    {
    }       

    public Texture2D Sprite
    {
      get => this.sprite;
      set
      {
        this.sprite = value;
        this.origin = new Vector2((float) (this.sprite.Width / 2), (float) (this.sprite.Height / 2));
      }
    }

    public bool FlipX { get; set; }

    protected override float LayerDepth
    {
      get
      {
        return (float) (1.0 - ((double) this.Transform.Position.Y + (double) this.LayerPosOffset + (double) (Tilemap.Instance.HalfHeight * 50)) / (double) (Tilemap.Instance.Height * 50));
      }
    }

    public float LayerPosOffset
    {
      private get
      {
        float valueOrDefault = this.layerPosOffset.GetValueOrDefault();
        if (this.layerPosOffset.HasValue)
          return valueOrDefault;
        float layerPosOffset = Game1.Random.NextFloat(0.0f, 0.25f);
        this.layerPosOffset = new float?(layerPosOffset);
        return layerPosOffset;
      }
      set => this.layerPosOffset = new float?(value);
    }

    public float ScaleMultiplier { private get; set; } = 1f;

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.sprite == null)
        return;
      this.DrawTexture(spriteBatch, this.Sprite, this.FlipX, this.ScaleMultiplier);
    }

    public void SetSprite(string currentAnimationSpriteName)
    {
      this.Sprite = Glob.Content.Load<Texture2D>(currentAnimationSpriteName);
    }
  }
}
