
// Type: GameManager.GameObjects.Components.Items.Weapons.Hammer
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Weapons
{
  public class Hammer : Weapon
  {
    public Hammer() 
    {
      this.Name = "Elemental Hammer";
      this.Damage = 5f;
      this.BaseDamage = this.Damage;
      this.Speed = 1f;
      this.AttackType = AttackType.Smash;
    }

    public Hammer(GameObject gameObject) : this()
    {
       
    }
  }
}
