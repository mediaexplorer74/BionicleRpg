
// Type: GameManager.GameObjects.Components.Animator
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Renderers;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


namespace GameManager.GameObjects.Components
{
  public class Animator : Component
  {
    private float elapsedTime;
    private SpriteRenderer spriteRenderer;
    private Texture2D oldSprite;
    private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
    private Animation currentAnimation;

    public bool IsOverlay { private get; set; }

    public int CurrentIndex { get; set; }

    public float FpsMultiplier { private get; set; } = 1f;

    public Animator()  
    { }

    public Animator(GameObject gameObject) : this() 
    { }

    public void Start() => this.spriteRenderer = this.GetComponent<SpriteRenderer>();

    public void OverrideSpriteRenderer(SpriteRenderer newSpriteRenderer)
    {
      this.spriteRenderer = newSpriteRenderer;
    }

    public void Update()
    {
      if (this.currentAnimation == null)
        return;
            
      this.elapsedTime += Glob.DeltaTime;
      this.CurrentIndex = (int) ((double) this.elapsedTime 
                * (double) this.currentAnimation.Fps * (double) this.FpsMultiplier);
      if (this.CurrentIndex > this.currentAnimation.SpriteNames.Length - 1)
      {
        this.elapsedTime = 0.0f;
        this.CurrentIndex = 0;
      }
      if (this.currentAnimation.RotationApplies)
      {
        SpriteRotation component = this.GetComponent<SpriteRotation>();
        SpriteRenderer spriteRenderer = this.spriteRenderer;
        string currentAnimationSpriteName;
        if (component == null)
        {
          currentAnimationSpriteName = this.currentAnimation.SpriteNames[this.CurrentIndex] ?? "";
        }
        else
        {
          /*DefaultInterpolatedStringHandler*/var interpolatedStringHandler = new StringBuilder();//new DefaultInterpolatedStringHandler(2, 2);
          interpolatedStringHandler.AppendFormat(this.currentAnimation.SpriteNames[this.CurrentIndex]);
          interpolatedStringHandler.Append("_0");
          currentAnimationSpriteName = interpolatedStringHandler.ToString();
          //interpolatedStringHandler.Clear();
          //interpolatedStringHandler.Append("_");
          interpolatedStringHandler.Append(component.RotationId.ToString());
          //interpolatedStringHandler.AppendFormat(component.RotationId.ToString());          
          currentAnimationSpriteName = interpolatedStringHandler.ToString();
        }
        spriteRenderer.SetSprite(currentAnimationSpriteName);
      }
      else
        this.spriteRenderer.SetSprite(this.currentAnimation.SpriteNames[this.CurrentIndex] ?? "");
    }

    public void AddAnimation(Animation animation) => this.animations.Add(animation.Name, animation);

    public void PlayAnimation(string animationName)
    {
      if (!this.animations.ContainsKey(animationName))
        return;
      if (this.oldSprite == null)
        this.oldSprite = this.spriteRenderer.Sprite;
      if (!(animationName != this.currentAnimation?.Name))
        return;
      this.currentAnimation = this.animations[animationName];
      this.elapsedTime = 0.0f;
      this.CurrentIndex = 0;
    }

    public float GetAnimFPS(string animationName)
    {
      return this.animations[animationName].Fps * this.FpsMultiplier;
    }

    public float GetAnimFrames(string animationName)
    {
      return (float) this.animations[animationName].SpriteNames.Length;
    }

    public void StopAnimation()
    {
      if (this.currentAnimation == null)
        return;
      this.spriteRenderer.Sprite = this.oldSprite;
      this.currentAnimation = (Animation) null;
    }
  }
}
