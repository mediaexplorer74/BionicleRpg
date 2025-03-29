
// Type: GameManager.GameObjects.Components.Items.Weapons.Sword
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Weapons
{
  public class Sword : Weapon
  {
    public Sword()
    {
      this.Name = "Elemental Sword";
      this.Damage = 3f;
      this.BaseDamage = this.Damage;
      this.Speed = 1f;
      this.DelayTimer = this.Speed;
      this.AttackType = AttackType.Slash;
    }

    public Sword(GameObject gameObject) : this()
    {
        this.GameObject = gameObject;
    }
  }
}
