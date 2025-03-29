
// Type: GameManager.GameObjects.Components.PlayerComponents.PlayerController
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

#nullable disable
namespace GameManager.GameObjects.Components.PlayerComponents
{
  public class PlayerController : Component
  {
    public PlayerController() 
    { }
    
    public PlayerController(GameObject gameObject) : this() 
    { }

    public float Rotation { get; private set; }

    public float AimDirection { get; private set; }

    public void Update()
    {
      InputHandler.Instance.Execute(this);
      this.MouseAim();
    }

    private void MouseAim()
    {
      MouseState state = Mouse.GetState();

      Vector2 vector2 = new Vector2((float) state.X 
          - (float) ((double) this.Transform.Position.X 
          - (double) Player.Instance.Transform.Position.X + (double) Game1.ScreenSize.X / 2.0),
          (float) state.Y - (float) ((double) this.Transform.Position.Y 
          - (double) Player.Instance.Transform.Position.Y 
          + (double) Game1.ScreenSize.Y / 2.0));

      this.AimDirection = (float) Math.Atan2((double) vector2.X, (double) vector2.Y);
      this.GetComponent<Combat>().AimDirection = this.AimDirection;

      this.Transform.WorldDirection 
                = MathHelper.ToDegrees(this.AimDirection);
    }
  }
}
