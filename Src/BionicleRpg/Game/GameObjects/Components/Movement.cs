
// Type: GameManager.GameObjects.Components.Movement
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Lighting;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Movement : Component
  {
    public float Speed { get; set; } = 200f;

    public Vector2 Velocity { get; set; }

    public Vector2 ExtraVelocity { get; set; }

    public bool CanMove { get; set; } = true;

        public Movement() 
        {
           
        }

        public Movement(GameObject gameObject) : this() { }

        public void Move(Vector2 velocity) => this.Velocity += velocity;

    public void LateUpdate()
    {
      if (this.CanMove)
      {
        if (this.Velocity != Vector2.Zero)
          this.Velocity.Normalize();
        float num = 1f;
        if (!Tilemap.IsPlayerLevitating)
        {
          Tilemap.Tile tile = Tilemap.Instance.GetTile(this.Transform.Position);
          if ((tile != null ? (tile.IsSlow ? 1 : 0) : 0) != 0 && this.GetComponent<Projectile>() == null)
            num = 0.25f;
        }
        this.Transform.Translate((this.Velocity * this.Speed * num + this.ExtraVelocity) * Glob.DeltaTime);
      }
      this.Velocity = Vector2.Zero;
    }

    public void OnCollision(Collider other)
    {
    }
  }
}
