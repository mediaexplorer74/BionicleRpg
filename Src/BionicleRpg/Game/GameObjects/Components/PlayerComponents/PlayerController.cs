
// Type: GameManager.GameObjects.Components.PlayerComponents.PlayerController
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;


namespace GameManager.GameObjects.Components.PlayerComponents
{
  public class PlayerController : Component
  {
     private MouseState OldMouseState = Mouse.GetState();
     private Vector2 V2 = new Vector2(0, 0);

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
        MouseState MouseState = Mouse.GetState();
        TouchCollection TouchState = TouchPanel.GetState();        

        if (TouchState.Count > 0 )
        {
            V2 = new Vector2((float)TouchState[0].Position.X / Game1.screenScale.X
                    - (float)((double)this.Transform.Position.X
                   - (double)Player.Instance.Transform.Position.X 
                     + (double)Game1.ScreenSize.X / 2.0),
                (float)TouchState[0].Position.Y / Game1.screenScale.Y
                - (float)((double)this.Transform.Position.Y
                   - (double)Player.Instance.Transform.Position.Y
                    + (double)Game1.ScreenSize.Y / 2.0));
        }
        else if (OldMouseState.Position.X != MouseState.Position.X ||
                OldMouseState.Position.Y != MouseState.Position.Y)
            {
            try
            {
                V2 = new Vector2((float)MouseState.X / Game1.screenScale.X
                        - (float)((double)this.Transform.Position.X
                    - (double)Player.Instance.Transform.Position.X
                    + (double)Game1.ScreenSize.X / 2.0),
                    (float)MouseState.Y / Game1.screenScale.Y
                    - (float)((double)this.Transform.Position.Y
                    - (double)Player.Instance.Transform.Position.Y
                    + (double)Game1.ScreenSize.Y / 2.0));
            }
            catch { }
       }

       this.AimDirection = (float) Math.Atan2((double) V2.X, (double) V2.Y);
       this.GetComponent<Combat>().AimDirection = this.AimDirection;

       this.Transform.WorldDirection 
                = MathHelper.ToDegrees(this.AimDirection);

       this.OldMouseState = MouseState;
     }
  }
}
