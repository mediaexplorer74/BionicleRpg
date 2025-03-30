
// Type: GameManager.GameObjects.Components.Items.Weapons.Weapon
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.ObjectPool;


namespace GameManager.GameObjects.Components.Items.Weapons
{
  public abstract class Weapon : Item
  {
    protected float FireTimer;
    protected float DelayTimer;
    public AttackType AttackType;
    public GameObject Owner;

    public float Damage { get; set; }

    public float BaseDamage { get; set; }

    public float Speed { get; set; }

    public Weapon()
    {
    }

    public Weapon(string name, float damage, float speed) : this()
    {
      this.Name = name;
      this.Damage = damage;
      this.BaseDamage = damage;
      this.Speed = speed;
    }

    public void Use() => this.SpawnProjectile();

    public void UpdateStats()
    {
      Combat component = this.Owner.GetComponent<Combat>();
      if (component != null)
      {
        if ((double) component.Strength == 1.0)
          this.Damage = this.BaseDamage;
        else
          this.Damage *= component.Strength;
        this.Speed = component.Speed;
      }
      this.DelayTimer = this.Speed;
    }

    private void SpawnProjectile()
    {
      if ((double) this.FireTimer > Glob.GameTime.TotalGameTime.TotalSeconds)
        return;
      this.FireTimer = (float) Glob.GameTime.TotalGameTime.TotalSeconds + this.DelayTimer;
      this.Owner.AddComponent<Audio>().Play("Fwip");

      GameObject gameObject = ProjectilePool.Instance.GetObject(
          this.Owner.GetComponent<Combat>().SelectedElement, this.AttackType, this.Owner);

      Projectile component1 = gameObject.GetComponent<Projectile>();
      component1.Owner = this.Owner;
      component1.Damage = this.Damage;
      Movement component2 = this.Owner.GetComponent<Movement>();
      gameObject.GetComponent<Movement>().ExtraVelocity = component2.Velocity * component2.Speed;
      gameObject.Transform.Position = this.Owner.Transform.Position - Tools.GetVectorFromAngle(
          this.Owner.Transform.WorldDirection) * 10f;

      gameObject.Transform.Rotation = 3.14159274f - this.Owner.GetComponent<Combat>().AimDirection;
    }
  }
}
