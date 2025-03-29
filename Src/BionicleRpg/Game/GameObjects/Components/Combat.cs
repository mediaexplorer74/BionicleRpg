
// Type: GameManager.GameObjects.Components.Combat
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Items.Weapons;
using System;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Combat : Component
  {
    public float AimDirection;
    public Weapon Weapon;
    public Element SelectedElement;
    private float elementalCost = 10f;
    public bool CanAttack = true;

    public float ElementalEnergy { get; set; } = 100f;

    public float MaxElementalEnergy { get; private set; } = 100f;

    public float Strength { get; set; }

    public float Speed { get; set; }

    private float DelayTimer { get; set; } = 0.5f;

    private float FireTimer { get; set; }

    private float RegenTimer { get; set; }

    public Combat() {}

    public Combat(GameObject gameObject) : this() { }

    public void Init(float strength, float speed)
    {
      this.Strength = strength;
      this.Speed = speed;
    }

    public void UseWeapon()
    {
      if (!this.CanAttack)
        return;
      this.Weapon?.Use();
    }

    public void UseElement()
    {
      if (!this.CanAttack || (double) this.ElementalEnergy < (double) this.elementalCost 
                || (double) this.FireTimer > Glob.GameTime.TotalGameTime.TotalSeconds)
        return;
      this.FireTimer = (float) Glob.GameTime.TotalGameTime.TotalSeconds + this.DelayTimer;
      this.GetComponent<ElementalAbility>().Use();
            this.ElementalEnergy = Math.Max(0.0f, Math.Min(this.ElementalEnergy - this.elementalCost, this.MaxElementalEnergy));
    }

    public void Update()
    {
      if ((double) this.RegenTimer > Glob.GameTime.TotalGameTime.TotalSeconds)
        return;
      this.RegenTimer = (float) Glob.GameTime.TotalGameTime.TotalSeconds + this.DelayTimer;
      if ((double) Math.Abs(this.ElementalEnergy - this.MaxElementalEnergy) < 0.0099999997764825821)
        return;
      this.ElementalEnergy += 0.25f;
    }
  }
}
