
// Type: GameManager.UI.Cooldown
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.UI
{
  public class Cooldown : UIComponent
  {
    private Image currentImg;
    private IconType imgOnCooldown;
    private readonly float timeLength;
    private float timeLeft;

    public bool Active { get; set; }

    public bool Repeat { get; set; }

    public Cooldown(Vector2 position, float length)
    {
      this.IsShowing = true;
      this.position = position;
      this.timeLength = length;
      this.timeLeft = length;
    }

    public void StartStopCooldown() => this.Active = !this.Active;

    public void Reset() => this.timeLeft = this.timeLength;

    public override void Update()
    {
      if (!this.Active)
        return;
      this.timeLeft -= (float) Glob.GameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.timeLeft > 0.0)
        return;
      if (this.Repeat)
      {
        this.Reset();
      }
      else
      {
        this.StartStopCooldown();
        this.timeLeft = 0.0f;
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this.Active)
      {
        spriteBatch.DrawString(UIManager.Instance.UIFont, this.timeLeft.ToString("#"), this.position, Color.White, 0.0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
        foreach (UIComponent uiComponent in UIComponent.UIComponents)
        {
          if (uiComponent is Image)
          {
            this.currentImg = (Image) uiComponent;
            switch (this.imgOnCooldown)
            {
              case IconType.Mask:
                if (this.currentImg.IconType == this.imgOnCooldown)
                {
                  this.currentImg.Color = Color.Gray;
                  continue;
                }
                continue;
              case IconType.ElementIcon:
                if (this.currentImg.IconType == this.imgOnCooldown)
                {
                  this.currentImg.Color = Color.Gray;
                  continue;
                }
                continue;
              case IconType.ElementPower:
                if (this.currentImg.IconType == this.imgOnCooldown)
                {
                  this.currentImg.Color = Color.Gray;
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
      else
      {
        if (this.currentImg == null)
          return;
        foreach (UIComponent uiComponent in UIComponent.UIComponents)
        {
          if (uiComponent is Image)
          {
            this.currentImg = (Image) uiComponent;
            if (this.currentImg.Color == Color.Gray)
              this.currentImg.Color = Color.White;
          }
        }
      }
    }

    public void StartCooldown(IconType imageType)
    {
      this.imgOnCooldown = imageType;
      if ((double) this.timeLeft == (double) this.timeLength)
      {
        this.StartStopCooldown();
      }
      else
      {
        if (this.Active || (double) this.timeLeft > 0.0)
          return;
        this.Reset();
      }
    }
  }
}
