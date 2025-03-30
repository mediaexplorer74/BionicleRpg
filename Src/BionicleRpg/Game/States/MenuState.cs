
// Type: GameManager.States.MenuState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.UI;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.States
{
  public class MenuState : IState
  {
    public void Enter() => UIManager.Instance.ShowUIComponent(UIStateAssign.StartMenu, true);

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void Update()
    {
    }

    public void Exit() => UIManager.Instance.ShowUIComponent(UIStateAssign.StartMenu, false);
  }
}
