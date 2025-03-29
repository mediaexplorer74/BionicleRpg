
// Type: GameManager.ObjectPool.ProjectilePool
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.ObjectPool
{
  public class ProjectilePool : GameManager.ObjectPool.ObjectPool
  {
    private static ProjectilePool instance;

    public static ProjectilePool Instance
    {
      get
      {
        if (ProjectilePool.instance == null)
          ProjectilePool.instance = new ProjectilePool();
        return ProjectilePool.instance;
      }
    }

    protected override void CleanUp(GameObject gameObject)
    {
    }

    public override GameObject CreateObject(Element element, AttackType attackType)
    {
      return ProjectileFactory.Instance.Create(element, attackType);
    }

    protected override void ObjectRetrieved(GameObject gameObject, GameObject owner)
    {
      gameObject.Transform.Rotation = owner.GetComponent<Combat>().AimDirection;
      gameObject.Transform.Position = owner.Transform.Position - Vector2.UnitY * 10f;
    }
  }
}
