
// Type: GameManager.GameObjects.Components.Projectile
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.ObjectPool;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Projectile : Component
  {
    public GameObject Owner;
    public Element SelectedElement;
    public AttackType AttackType;
    public float Damage;
    private Animator animator;
    public float LifeSpan;
    private float totalLifeTime;

    public Projectile() 
    { }

    public Projectile(GameObject gameObject) : this() 
    { }

    public void Start()
    {
      this.animator = this.GetComponent<Animator>();
      this.animator?.PlayAnimation("Anim");
    }

    public void Update()
    {
      Movement component = this.GetComponent<Movement>();
      Vector2 vectorFromAngle = Tools.GetVectorFromAngle(this.Transform.Rotation - 1.57079637f);

      component?.Move(vectorFromAngle);

      if ((double) this.LifeSpan == 0.0)
        this.LifeSpan = this.animator.GetAnimFrames("Anim") / this.animator.GetAnimFPS("Anim");

      this.totalLifeTime += (float) Glob.GameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.totalLifeTime < (double) this.LifeSpan)
        return;

      if (component != null)
        component.Speed = 250f;

      ProjectilePool.Instance.ReleaseObject(this.SelectedElement, this.AttackType, this.GameObject);

      this.totalLifeTime = 0.0f;
    }
  }
}
