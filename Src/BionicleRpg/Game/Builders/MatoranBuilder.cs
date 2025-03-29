
// Type: GameManager.Builders.MatoranBuilder
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
  public class MatoranBuilder
  {
    public static MatoranBuilder Instance { get; } = new MatoranBuilder();

    private MatoranBuilder()
    {
    }

    public GameObject NewMatoran(Vector2 pos)
    {
      GameObject gameObject = new GameObject();
      gameObject.Transform.Position = pos;
      Matoran matoran = gameObject.AddComponent<Matoran>();
      gameObject.AddComponent<SpriteRenderer>().Sprite = Glob.Content.Load<Texture2D>("Matoran_Idle_F01_01");
      matoran.MaskSprite = gameObject.AddComponent<SpriteRenderer>();
      Animator animator = gameObject.AddComponent<Animator>();
      matoran.SpriteRotation = gameObject.AddComponent<SpriteRotation>();
      gameObject.AddComponent<Movement>();
      gameObject.AddComponent<Combat>();
      gameObject.AddComponent<Seeker>();
      gameObject.AddComponent<Health>();
      gameObject.AddComponent<Healthbar>();
      gameObject.GetComponent<Healthbar>().Awake();
      gameObject.AddComponent<NameDisplay>().Awake();
      SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
      spriteRenderer.Sprite = Glob.Content.Load<Texture2D>("FikouFloorShadow");
      spriteRenderer.SpriteBatchOverride = Game1.FloorSpriteBatch;
      spriteRenderer.LayerPosOffset = 100f;
      Animation animation1 = new Animation("Idle", 10f, new string[1]
      {
        "Matoran_Idle_F01"
      }, new bool?(true));
      Animation animation2 = new Animation("Walk", 10f, new string[2]
      {
        "Matoran_Walk_F01",
        "Matoran_Walk_F02"
      }, new bool?(true));
      animator.Start();
      animator.AddAnimation(animation1);
      animator.AddAnimation(animation2);
      animator.PlayAnimation("Idle");
      gameObject.Transform.Scale = 0.5f;
      return gameObject;
    }
  }
}
