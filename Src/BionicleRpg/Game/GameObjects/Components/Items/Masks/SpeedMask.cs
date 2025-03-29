
// Type: GameManager.GameObjects.Components.Items.Masks.SpeedMask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class SpeedMask : Mask
  {
    private float baseAttackSpeed;
    private float baseMoveSpeed;
    private Combat combatComponent;
    private Movement movementComponent;
    private Audio audioComponent;

    public SpeedMask() 
    {
        this.Name = "Kakama - Mask of speed";
        this.MaskSprite = "Kakama";
        this.MaskUiSprite = "Icon_SpeedMask";
        this.MaskEnergyConsumption = 10f;
        this.energyRegain = 4f;
        this.MinMaskEnergy = this.MaskEnergyConsumption;
    }
    
    public SpeedMask(GameObject gameObject) : this()
    {
         
    }

    public override void ActivateMaskPower()
    {
      if ((double) this.MaskEnergy < (double) this.MinMaskEnergy)
        return;
      this.combatComponent = this.Owner.GetComponent<Combat>();
      this.baseAttackSpeed = this.combatComponent.Speed;
      this.combatComponent.Speed /= 10f;
      this.movementComponent = this.Owner.GetComponent<Movement>();
      this.baseMoveSpeed = this.movementComponent.Speed;
      this.movementComponent.Speed *= 4f;
      this.audioComponent = this.Owner.GetComponent<Audio>();
      this.audioComponent?.Play("Short Acceleration");
      base.ActivateMaskPower();
    }

    public override void DeactivateMaskPower()
    {
      this.combatComponent.Speed = this.baseAttackSpeed;
      this.movementComponent.Speed = this.baseMoveSpeed;
      base.DeactivateMaskPower();
    }

    public override void EquipMask()
    {
    }
  }
}
