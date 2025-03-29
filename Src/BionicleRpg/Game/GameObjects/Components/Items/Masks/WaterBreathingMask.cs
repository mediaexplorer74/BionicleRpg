
// Type: GameManager.GameObjects.Components.Items.Masks.WaterBreathingMask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Tilemaps;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class WaterBreathingMask : Mask
  {
    private Audio audioComponent;

      
        public WaterBreathingMask()
        {
            this.Name = "Kaukau - Mask of water breathing";
            this.MaskSprite = "Kaukau";
            this.MaskUiSprite = "Icon_WaterBreathingMask";
            this.energyRegain = 5f;
        }

        public WaterBreathingMask(GameObject gameObject) : this()
        {
            this.GameObject = gameObject;           
        }

        public override void ActivateMaskPower()
    {
      if ((double) this.MaskEnergy < (double) this.MaxMaskEnergy / 2.0)
        return;
      Tilemap.WaterCollisionEnabled = false;
      this.audioComponent = this.Owner.GetComponent<Audio>();
      this.audioComponent?.Play("Mask Use");
      base.ActivateMaskPower();
    }

    public override void DeactivateMaskPower()
    {
      Tilemap.WaterCollisionEnabled = true;
      base.DeactivateMaskPower();
    }

    public override void EquipMask()
    {
    }
  }
}
