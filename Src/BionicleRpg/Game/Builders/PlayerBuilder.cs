
// Type: GameManager.Builders.PlayerBuilder
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.Lighting;
using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager.Builders
{
  public class PlayerBuilder : IBuilder
  {
    private static PlayerBuilder instance;
    private GameObject playerGameObject;

    public static PlayerBuilder Instance
    {
      get
      {
        if (PlayerBuilder.instance == null)
          PlayerBuilder.instance = new PlayerBuilder();
        return PlayerBuilder.instance;
      }
    }

    private PlayerBuilder()
    {
    }

    public void BuildGameObject()
    {
      this.playerGameObject = new GameObject();
      this.playerGameObject.AddComponent<Player>();
      this.playerGameObject.AddComponent<SpriteRotation>();
      this.playerGameObject.Transform.Scale = 0.675f;
      this.playerGameObject.AddComponent<VisibilityProvider>();
      Collider collider = this.playerGameObject.AddComponent<Collider>();
      collider.SizeOverride = new Rectangle(0, 0, 50, 50);
      collider.IsEnabled = true;
      this.playerGameObject.AddComponent<PlayerStats>();
      this.playerGameObject.AddComponent<Audio>();
      this.ConstructAnimator();
    }

    private void ConstructAnimator()
    {
      Animator animator = this.playerGameObject.AddComponent<Animator>();
      Animation animation1 = new Animation("Idle", 10f, new string[1]
      {
        "Toa_Idle_F01"
      }, new bool?(true));
      animator.AddAnimation(animation1);
      Animation animation2 = new Animation("Run", 3f, new string[2]
      {
        "Toa_Walk_F01",
        "Toa_Walk_F02"
      }, new bool?(true));
      animator.AddAnimation(animation2);
    }

    public GameObject GetResult() => this.playerGameObject;
  }
}
