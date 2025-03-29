
// Type: GameManager.Factories.ProjectileFactory
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.Factories
{
  public class ProjectileFactory : Factory
  {
    private static ProjectileFactory instance;
    private SpriteRenderer projectileSprite;
    private Element selectedElement;

    public static ProjectileFactory Instance
    {
      get => ProjectileFactory.instance ?? (ProjectileFactory.instance = new ProjectileFactory());
    }

    public override GameObject Create(Element element, AttackType attackType)
    {
      GameObject gameObject = new GameObject();
      this.selectedElement = element;
      this.projectileSprite = gameObject.AddComponent<SpriteRenderer>();
      this.projectileSprite.Sprite = Glob.Content.Load<Texture2D>("Slash1");
      Animator animator = gameObject.AddComponent<Animator>();
      AttackCollision attackCollision = gameObject.AddComponent<AttackCollision>();
      Movement movement = gameObject.AddComponent<Movement>();
      Projectile projectile = gameObject.AddComponent<Projectile>();
      projectile.SelectedElement = this.selectedElement;
      projectile.AttackType = attackType;
      gameObject.Transform.Scale = 2f;
      movement.Speed = 250f;
      this.CreateAnimations(attackType, animator);
      attackCollision.Start();
      return gameObject;
    }

    private void SetElementColor()
    {
      switch (this.selectedElement)
      {
        case Element.Fire:
          this.projectileSprite.Color = Color.Red;
          break;
        case Element.Water:
          this.projectileSprite.Color = Color.LightBlue;
          break;
        case Element.Ice:
          this.projectileSprite.Color = Color.White;
          break;
        case Element.Air:
          this.projectileSprite.Color = Color.LightYellow;
          break;
        case Element.Earth:
          this.projectileSprite.Color = Color.Purple;
          break;
        case Element.Stone:
          this.projectileSprite.Color = Color.Orange;
          break;
      }
    }

    private void CreateAnimations(AttackType attackType, Animator animator)
    {
      Animation animation;
      switch (attackType)
      {
        case AttackType.Slash:
          animation = new Animation("Anim", 20f, new string[4]
          {
            "Slash1",
            "Slash2",
            "Slash3",
            "Slash4"
          }, new bool?());
          this.SetElementColor();
          break;
        case AttackType.Stab:
          animation = new Animation("Anim", 20f, new string[4]
          {
            "Stab1",
            "Stab2",
            "Stab3",
            "Stab4"
          }, new bool?());
          this.SetElementColor();
          break;
        case AttackType.Smash:
          animation = new Animation("Anim", 20f, new string[4]
          {
            "Smash1",
            "Smash2",
            "Smash3",
            "Smash4"
          }, new bool?());
          this.SetElementColor();
          break;
        case AttackType.ElementalFire:
          animation = new Animation("Anim", 10f, new string[2]
          {
            "ElementalFire1",
            "ElementalFire2"
          }, new bool?());
          break;
        case AttackType.ElementalWater:
          animation = new Animation("Anim", 10f, new string[2]
          {
            "ElementalWater1",
            "ElementalWater2"
          }, new bool?());
          break;
        case AttackType.ElementalIce:
          animation = new Animation("Anim", 10f, new string[2]
          {
            "ElementalIce1",
            "ElementalIce2"
          }, new bool?());
          break;
        case AttackType.ElementalStone:
          animation = new Animation("Anim", 10f, new string[2]
          {
            "ElementalStone1",
            "ElementalStone2"
          }, new bool?());
          break;
        case AttackType.ElementalEarth:
          animation = new Animation("Anim", 10f, new string[2]
          {
            "ElementalEarth1",
            "ElementalEarth2"
          }, new bool?());
          break;
        case AttackType.ElementalAir:
          animation = new Animation("Anim", 10f, new string[2]
          {
            "ElementalAir1",
            "ElementalAir2"
          }, new bool?());
          break;
        default:
          animation = (Animation) null;
          break;
      }
      if (animation == null)
        return;
      animator.AddAnimation(animation);
    }
  }
}
