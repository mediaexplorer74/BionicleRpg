
// Type: GameManager.GameObjects.Components.ElementalAbility
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.UI;
using GameManager.ObjectPool;
using GameManager.UI;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class ElementalAbility : Component
  {
    private GameObject owner;
    private AttackType attackType;
    private float lifeSpan;
    private Audio audioComponent;

    public float Damage { get; set; }

    public float DelayTimer { get; set; }

    public float FireTimer { get; set; }

    public ElementalAbility() { }

    public ElementalAbility(GameObject gameObject) : this() { }

    public void Init()
    {
      this.owner = this.GameObject;
      switch (this.owner.GetComponent<Player>().SelectedElement)
      {
        case Element.Fire:
          this.attackType = AttackType.ElementalFire;
          this.DelayTimer = 0.3f;
          this.lifeSpan = 10f;
          this.Damage = 10f;
          UIManager.Instance.UpdateSprite(IconType.ElementPower, "FireIcon");
          break;
        case Element.Water:
          this.attackType = AttackType.ElementalWater;
          this.DelayTimer = 0.4f;
          this.lifeSpan = 10f;
          this.Damage = 10f;
          UIManager.Instance.UpdateSprite(IconType.ElementPower, "WaterIcon");
          break;
        case Element.Ice:
          this.attackType = AttackType.ElementalIce;
          this.DelayTimer = 0.3f;
          this.lifeSpan = 10f;
          this.Damage = 10f;
          UIManager.Instance.UpdateSprite(IconType.ElementPower, "IceIcon");
          break;
        case Element.Air:
          this.attackType = AttackType.ElementalAir;
          this.DelayTimer = 0.5f;
          this.lifeSpan = 3f;
          this.Damage = 10f;
          UIManager.Instance.UpdateSprite(IconType.ElementPower, "AirIcon");
          break;
        case Element.Earth:
          this.attackType = AttackType.ElementalEarth;
          this.DelayTimer = 1f;
          this.lifeSpan = 3f;
          this.Damage = 10f;
          UIManager.Instance.UpdateSprite(IconType.ElementPower, "EarthIcon");
          break;
        case Element.Stone:
          this.attackType = AttackType.ElementalStone;
          this.DelayTimer = 0.8f;
          this.lifeSpan = 3f;
          this.Damage = 10f;
          UIManager.Instance.UpdateSprite(IconType.ElementPower, "StoneIcon");
          break;
      }
    }

    public void Use()
    {
      GameObject gameObject = ProjectilePool.Instance.GetObject(this.owner.GetComponent<Combat>().SelectedElement, this.attackType, this.owner);
      Projectile component = gameObject.GetComponent<Projectile>();
      component.Owner = this.owner;
      component.Damage = this.Damage;
      component.LifeSpan = this.lifeSpan;
      this.audioComponent = this.owner.GetComponent<Audio>();
      this.audioComponent?.Play("Element Use");
      gameObject.GetComponent<Movement>().Speed += (float) ((double) this.owner.GetComponent<Movement>().Velocity.Length() * 250.0 + 50.0);
      gameObject.Transform.Position = this.owner.Transform.Position - Tools.GetVectorFromAngle(this.owner.Transform.WorldDirection) * 10f;
      gameObject.Transform.Rotation = 3.14159274f - this.owner.GetComponent<Combat>().AimDirection;
    }
  }
}
