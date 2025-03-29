
// Type: GameManager.GameObjects.Components.Items.Masks.XrayMask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Lighting;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class XrayMask : Mask
  {
    private Audio audioComponent;

    public XrayMask() 
    {
        this.Name = "Akaku - Xray mask";
        this.MaskSprite = "Akaku";
        this.MaskUiSprite = "Icon_XrayMask";
        this.energyRegain = 5f;
        this.MinMaskEnergy = this.MaxMaskEnergy / 2f;
    }

    public XrayMask(GameObject gameObject): this ()
    {
        
    }

    public override void ActivateMaskPower()
    {
      if ((double) this.MaskEnergy < (double) this.MinMaskEnergy)
        return;
      this.ChangeVisionColor(new Color(0.25f, 0.0f, 0.0f, 0.5f));
      this.audioComponent = this.Owner.GetComponent<Audio>();
      this.audioComponent?.Play("Mask Use");
      base.ActivateMaskPower();
    }

    public override void DeactivateMaskPower()
    {
      this.ChangeVisionColor(VisionTilemap.OriginalVisionColor);
      base.DeactivateMaskPower();
    }

    public override void EquipMask()
    {
    }

    private void ChangeVisionColor(Color color)
    {
      VisibilityProvider component = this.Owner.GetComponent<VisibilityProvider>();
      component.IsEnabled = false;
      VisionTilemap.Instance.SetVisionColor(color);
      component.IsEnabled = true;
    }
  }
}
