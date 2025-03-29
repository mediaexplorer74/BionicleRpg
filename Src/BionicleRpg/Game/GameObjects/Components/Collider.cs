
// Type: GameManager.GameObjects.Components.Collider
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Collider : Component
  {
    private static readonly List<Collider> colliders = new List<Collider>();
    private SpriteRenderer spriteRenderer;
    private static Texture2D texture;

    public static ReadOnlyCollection<Collider> Colliders => Collider.colliders.AsReadOnly();

    public Rectangle CollisionBox
    {
      get
      {
        return this.SizeOverride != new Rectangle() ? new Rectangle((int) ((double) this.GameObject.Transform.Position.X - (double) (this.SizeOverride.Width / 2) * (double) this.Transform.Scale), (int) ((double) this.GameObject.Transform.Position.Y - (double) (this.SizeOverride.Height / 2) * (double) this.Transform.Scale), (int) ((double) this.SizeOverride.Width * (double) this.Transform.Scale), (int) ((double) this.SizeOverride.Height * (double) this.Transform.Scale)) : new Rectangle((int) ((double) this.GameObject.Transform.Position.X - (double) (this.spriteRenderer.Sprite.Width / 2) * (double) this.Transform.Scale), (int) ((double) this.GameObject.Transform.Position.Y - (double) (this.spriteRenderer.Sprite.Height / 2) * (double) this.Transform.Scale), (int) ((double) this.spriteRenderer.Sprite.Width * (double) this.Transform.Scale), (int) ((double) this.spriteRenderer.Sprite.Height * (double) this.Transform.Scale));
      }
    }

    public Rectangle SizeOverride { private get; set; }

   

    public void OnDestroy() => Collider.colliders.Remove(this);


    public Collider()
    {
       this.IsEnabled = false;
    }

    public Collider(GameObject gameObject) : this() 
    { }

    public void Start()
    {
      this.spriteRenderer = this.GameObject.GetComponent<SpriteRenderer>();
      Collider.texture = Glob.Content.Load<Texture2D>("pixel");
    }

    public void LastUpdate() => this.CheckCollision();

    public void OnEnable() => Collider.colliders.Add(this);

    public void OnDisable() => Collider.colliders.Remove(this);

    public void Draw(SpriteBatch spriteBatch)
    {
      Texture2D sprite = this.spriteRenderer?.Sprite;
    }

    public static void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
    {
      Rectangle destinationRectangle1 = new Rectangle(collisionBox.X - (int) Player.Instance.Transform.Position.X + (int) Game1.ScreenSize.X / 2, collisionBox.Y - (int) Player.Instance.Transform.Position.Y + (int) Game1.ScreenSize.Y / 2, collisionBox.Width, 1);
      Rectangle destinationRectangle2 = new Rectangle(collisionBox.X - (int) Player.Instance.Transform.Position.X + (int) Game1.ScreenSize.X / 2, collisionBox.Y + collisionBox.Height - (int) Player.Instance.Transform.Position.Y + (int) Game1.ScreenSize.Y / 2, collisionBox.Width, 1);
      Rectangle destinationRectangle3 = new Rectangle(collisionBox.X - (int) Player.Instance.Transform.Position.X + (int) Game1.ScreenSize.X / 2, collisionBox.Y - (int) Player.Instance.Transform.Position.Y + (int) Game1.ScreenSize.Y / 2, 1, collisionBox.Height);
      Rectangle destinationRectangle4 = new Rectangle(collisionBox.X + collisionBox.Width - (int) Player.Instance.Transform.Position.X + (int) Game1.ScreenSize.X / 2, collisionBox.Y - (int) Player.Instance.Transform.Position.Y + (int) Game1.ScreenSize.Y / 2, 1, collisionBox.Height);
      spriteBatch.Draw(Collider.texture, destinationRectangle1, new Rectangle?(), Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
      spriteBatch.Draw(Collider.texture, destinationRectangle2, new Rectangle?(), Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
      spriteBatch.Draw(Collider.texture, destinationRectangle4, new Rectangle?(), Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
      spriteBatch.Draw(Collider.texture, destinationRectangle3, new Rectangle?(), Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
    }

    private void CheckCollision()
    {
      if (this.spriteRenderer?.Sprite == null || Player.Instance.GameObject != this.GameObject)
        return;
      foreach (Collider other in Collider.colliders.ToList<Collider>())
      {
        if (other != this && other.spriteRenderer?.Sprite != null && other.CollisionBox.Intersects(this.CollisionBox))
        {
          this.GameObject.OnCollision(other);
          this.PlayerCollide(other);
        }
      }
    }

    private void PlayerCollide(Collider other)
    {
      if ((double) Math.Abs(this.Transform.Position.Y - other.Transform.Position.Y) > (double) Math.Abs(this.Transform.Position.X - other.Transform.Position.X))
      {
        if ((double) this.Transform.Position.Y > (double) other.Transform.Position.Y)
          this.Transform.Translate(new Vector2(0.0f, (float) Rectangle.Intersect(this.CollisionBox, other.CollisionBox).Height));
        else
          this.Transform.Translate(new Vector2(0.0f, (float) -Rectangle.Intersect(this.CollisionBox, other.CollisionBox).Height));
      }
      else if ((double) this.Transform.Position.X > (double) other.Transform.Position.X)
        this.Transform.Translate(new Vector2((float) Rectangle.Intersect(this.CollisionBox, other.CollisionBox).Width, 0.0f));
      else
        this.Transform.Translate(new Vector2((float) -Rectangle.Intersect(this.CollisionBox, other.CollisionBox).Width, 0.0f));
    }

    public static void ClearAll() => Collider.colliders.Clear();
  }
}
