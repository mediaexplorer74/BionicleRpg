
// Type: GameManager.GameObjects.Components.Items.Masks.StrengthMask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class StrengthMask : Mask
  {
    private float baseStrength;
    private Combat combatComponent;
    private Audio audioComponent;

    public StrengthMask() //: base()
    {
      this.Name = "Pakari - Mask of strength";
      this.MaskSprite = "Pakari";
      this.MaskUiSprite = "Icon_StrengthMask";
      this.MinMaskEnergy = this.MaxMaskEnergy / 2f;
    }

    public StrengthMask(GameObject gameObject) : this()
    {
       this.GameObject = gameObject;            
    }

    public override void ActivateMaskPower()
    {
      if ((double) this.MaskEnergy < (double) this.MinMaskEnergy)
        return;
      this.combatComponent = this.Owner.GetComponent<Combat>();
      this.baseStrength = this.combatComponent.Strength;
      this.combatComponent.Strength = 5f;
      this.audioComponent = this.Owner.GetComponent<Audio>();
      this.audioComponent?.Play("Mask Use");
      base.ActivateMaskPower();
    }

    public override void DeactivateMaskPower()
    {
      this.combatComponent.Strength = 1f;
      base.DeactivateMaskPower();
    }

    public override void EquipMask()
    {
    }
  }
}
