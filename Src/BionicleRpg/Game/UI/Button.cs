
// Type: GameManager.UI.Button
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace GameManager.UI
{
  public class Button : UIComponent
  {
    private MouseState currentMouse;
    private MouseState previousMouse;
    private Color color;
    private bool isHovering;

    public event HoverEventHandler HoverEvent;

    public event EventHandler Click;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int) 
            ((double) this.position.X - (double) (this.Sprite.Width / 2)), 
            (int) ((double) this.position.Y - (double) this.Sprite.Height), 
            (int) ((double) this.Sprite.Width * (double) this.scale), 
            (int) ((double) this.Sprite.Height * (double) this.scale));
      }
    }

    public Button(string spriteName, Vector2 position)
    {
      this.spriteName = spriteName;
      this.position = position;
      this.scale = 1f;
    }

    public override void Awake()
    {
      this.Sprite = Glob.Content.Load<Texture2D>(this.spriteName);
      this.origin = new Vector2((float) (this.Sprite.Width / 2), (float) (this.Sprite.Height / 2));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      this.color = Color.White;
      if (this.isHovering)
        this.OnHoverEvent();
      spriteBatch.Draw(this.Sprite, this.position, new Rectangle?(),
          this.color, 0.0f, this.origin, this.scale, SpriteEffects.None, 0.0f);
    }

    public override void Update()
    {
      this.previousMouse = this.currentMouse;
      this.currentMouse = Mouse.GetState();
      Rectangle rectangle = new Rectangle(this.currentMouse.X, this.currentMouse.Y, 1, 1);
      this.isHovering = false;
      if (!rectangle.Intersects(this.Rectangle))
        return;
      this.isHovering = true;
      if (this.currentMouse.LeftButton != ButtonState.Released 
                || this.previousMouse.LeftButton != ButtonState.Pressed)
        return;
      this.OnClickEvent();
    }

    private void OnHoverEvent()
    {
      this.color = Color.DarkGray;
      HoverEventHandler hoverEvent = this.HoverEvent;
      if (hoverEvent == null)
        return;
      hoverEvent(this);
    }

    private void OnClickEvent()
    {
      EventHandler click = this.Click;
      if (click == null)
        return;
      click((object) this, new EventArgs());
    }
  }
}
