
// Type: GameManager.Builders.EnemyBuilder
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.Builders
{
  public class EnemyBuilder
  {
    public static EnemyBuilder Instance { get; } = new EnemyBuilder();

    private EnemyBuilder()
    {
    }

    public GameObject NewEnemy(Vector2 pos)
    {
      GameObject gameObject = new GameObject();
      gameObject.Transform.Position = pos;
      gameObject.AddComponent<Enemy>();
      gameObject.AddComponent<SpriteRenderer>().Sprite = Glob.Content.Load<Texture2D>("Fikou_01");
      Animator animator = gameObject.AddComponent<Animator>();
      gameObject.AddComponent<SpriteRotation>();
      gameObject.AddComponent<Movement>();
      gameObject.AddComponent<Combat>();
      gameObject.AddComponent<Seeker>();
      gameObject.AddComponent<Health>();
      gameObject.AddComponent<Healthbar>();
      gameObject.GetComponent<Healthbar>().Awake();
      gameObject.AddComponent<NameDisplay>().Awake();
      gameObject.AddComponent<Audio>();
      SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
      spriteRenderer.Sprite = Glob.Content.Load<Texture2D>("FikouFloorShadow");
      spriteRenderer.SpriteBatchOverride = Game1.FloorSpriteBatch;
      spriteRenderer.LayerPosOffset = 100f;
      Animation animation = new Animation("Idle", 10f, new string[1]
      {
        "Fikou"
      }, new bool?(true));
      animator.Start();
      animator.AddAnimation(animation);
      animator.PlayAnimation("Idle");
      gameObject.Transform.Scale = 0.5f;
      return gameObject;
    }
  }
}
