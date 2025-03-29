
// Type: GameManager.GameObjects.Components.Items.Masks.LevitationMask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class LevitationMask : Mask
  {
    private const float AnimationTime = 0.5f;
    private static readonly Vector2 levitationOffset = new Vector2(0.0f, -75f);
    private bool isActive;
    private float timeSinceStateChanged;
    private Vector2 originOffset;
    private Audio audioComponent;

    public LevitationMask() 
    {
      
    }

    public LevitationMask(GameObject gameObject) : this()
    {
        this.Name = "Miru - Mask of levitation";
        this.MaskSprite = "Miru";
        this.MaskUiSprite = "Icon_LevitationMask";
        this.MaskEnergyConsumption = 15f;
        this.energyRegain = 3f;
        this.MinMaskEnergy = this.MaskEnergyConsumption;
    }

    public override void ActivateMaskPower()
    {
      if ((double) this.MaskEnergy < (double) this.MinMaskEnergy)
        return;
      this.SetState(true);
      Player.Instance.SetToaLayerOffset(100f);
      this.audioComponent = this.Owner.GetComponent<Audio>();
      this.audioComponent?.Play("Slow Levitated jump");
      base.ActivateMaskPower();
    }

    public override void DeactivateMaskPower()
    {
      this.SetState(false);
      Player.Instance.SetToaLayerOffset(0.0f);
      base.DeactivateMaskPower();
    }

    private void SetState(bool value)
    {
      this.originOffset = Player.Instance.GetToaOffset();
      this.timeSinceStateChanged = 0.0f;
      this.isActive = value;
      Tilemap.IsPlayerLevitating = value;
      this.Owner.GetComponent<Health>().IsInvincible = value;
      this.Owner.GetComponent<Combat>().CanAttack = !value;
    }

    public new void Update()
    {
      base.Update();
      this.timeSinceStateChanged += Glob.DeltaTime;
      Vector2 vector2 = this.isActive ? LevitationMask.levitationOffset : Vector2.Zero;
      float t = this.timeSinceStateChanged / 0.5f;
      Player.Instance.SetToaOffset(new Vector2(
          Tools.SmoothStep(this.originOffset.X, vector2.X, t), Tools.SmoothStep(this.originOffset.Y, vector2.Y, t)));
    }

    public override void EquipMask()
    {
    }
  }
}
