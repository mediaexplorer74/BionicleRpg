
// Type: GameManager.GameObjects.Components.AttackCollision
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Renderers;
using GameManager.GameObjects.Components.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace GameManager.GameObjects.Components
{
  public class AttackCollision : Component
  {
    private List<RectangleData> rectangles = new List<RectangleData>();
    private Texture2D texture;
    private SpriteRenderer spriteRenderer;

    public AttackCollision()
    { }

    public AttackCollision(GameObject gameObject) : this() 
    {
       
    }

    public void Start()
    {
      this.spriteRenderer = this.GameObject.GetComponent<SpriteRenderer>();
      this.texture = Glob.Content.Load<Texture2D>("Pixel");
    }

    public Rectangle CollisionBox
    {
      get
      {
        return new Rectangle((int) ((double) this.GameObject.Transform.Position.X - (double) (this.spriteRenderer.Sprite.Width / 2) * (double) this.Transform.Scale),
            (int) ((double) this.GameObject.Transform.Position.Y - (double) (this.spriteRenderer.Sprite.Height / 2) * (double) this.Transform.Scale), 
            (int) ((double) this.spriteRenderer.Sprite.Width * (double) this.Transform.Scale), 
            (int) ((double) this.spriteRenderer.Sprite.Width * (double) this.Transform.Scale));
      }
    }

    public void Update() => this.CheckCollision();

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    private void CheckCollision()
    {
      Projectile component = this.GetComponent<Projectile>();
      if (component.Owner.GetComponent<Enemy>() == null)
      {
        foreach (Enemy enemy in Enemy.Enemies)
        {
          if (enemy.CollisionBox.Intersects(this.CollisionBox))
            this.GameObject.OnCollision(enemy, component.Damage);
        }
        foreach (Collider collider in Collider.Colliders.ToList<Collider>())
        {
          if (collider.CollisionBox.Intersects(this.CollisionBox))
            collider.GetComponent<Resource>()?.ExtractResource();
        }
      }
      else
      {
        if (!Player.Instance.GetComponent<Collider>().CollisionBox.Intersects(this.CollisionBox))
          return;
        Player.Instance.GameObject.OnCollision(Player.Instance, component.Damage);
      }
    }
  }
}
