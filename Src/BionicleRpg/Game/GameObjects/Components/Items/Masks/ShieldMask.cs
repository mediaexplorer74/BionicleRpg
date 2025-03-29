
// Type: GameManager.GameObjects.Components.Items.Masks.ShieldMask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Renderers;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
  public class ShieldMask : Mask
  {
    private Movement movementComponent;
    private float baseMoveSpeed;
    private Animator shieldAnimator;
    private SpriteRenderer shieldRenderer;
    private Animation ShieldAnim;
    private Audio audioComponent;

    public ShieldMask() 
    {
      this.Name = "Hau - Mask of shielding";
      this.MaskSprite = "Hau";
      this.MaskUiSprite = "Icon_ShieldMask";
      this.MinMaskEnergy = this.MaxMaskEnergy / 2f;
    }

    public ShieldMask(GameObject owner) : this()
    {
        this.Owner = owner;        
    }

    public override void ActivateMaskPower()
    {
      if ((double) this.MaskEnergy < (double) this.MinMaskEnergy)
        return;
      this.ConstructVisuals();
      this.Owner.GetComponent<Health>().IsInvincible = true;
      this.Owner.GetComponent<Combat>().CanAttack = false;
      this.movementComponent = this.Owner.GetComponent<Movement>();
      this.baseMoveSpeed = this.movementComponent.Speed;
      this.movementComponent.Speed /= 2f;
      this.audioComponent = this.Owner.GetComponent<Audio>();
      this.audioComponent?.Play("Mask Use");
      base.ActivateMaskPower();
    }

    private void ConstructVisuals()
    {
      this.shieldAnimator = this.Owner.AddComponent<Animator>();
      this.shieldRenderer = this.Owner.AddComponent<SpriteRenderer>();
      this.shieldRenderer.Color = this.Owner.GetComponent<Player>().GetEyeColor();
      this.ConstructAnims();
      this.shieldAnimator.OverrideSpriteRenderer(this.shieldRenderer);
      this.shieldAnimator.PlayAnimation("Anim");
    }

    private void ConstructAnims()
    {
      this.ShieldAnim = new Animation("Anim", 3f, new string[2]
      {
        "Shield1",
        "Shield2"
      }, new bool?(false));
      this.shieldAnimator.AddAnimation(this.ShieldAnim);
      this.shieldAnimator.Start();
    }

    public override void DeactivateMaskPower()
    {
      this.Owner.GetComponent<Health>().IsInvincible = false;
      this.Owner.GetComponent<Combat>().CanAttack = true;
      this.movementComponent.Speed = this.baseMoveSpeed;
      this.Owner.RemoveComponent((Component) this.shieldAnimator);
      this.Owner.RemoveComponent((Component) this.shieldRenderer);
      base.DeactivateMaskPower();
    }
  }
}
