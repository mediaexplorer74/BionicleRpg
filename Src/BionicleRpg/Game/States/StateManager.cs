
// Type: GameManager.States.StateManager
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace GameManager.States
{
  public class StateManager
  {
    private Stack<IState> screens = new Stack<IState>();
    private static StateManager instance;

    public static StateManager Instance
    {
      get
      {
        if (StateManager.instance == null)
          StateManager.instance = new StateManager();
        return StateManager.instance;
      }
    }

    public IState CurrentState => this.screens.Peek();

    public void AddScreen(IState screen)
    {
      if (this.screens.Count > 0)
        this.screens.Peek().Exit();
      this.screens.Push(screen);
      this.screens.Peek().Enter();
    }

    public void RemoveScreen()
    {
      if (this.screens.Count <= 0)
        return;
      this.screens.Pop().Exit();
      this.screens.Peek().Enter();
    }

    public void ClearScreens()
    {
      while (this.screens.Count > 0)
        this.screens.Pop().Exit();
    }

    public void ChangeScreen(IState screen)
    {
      this.ClearScreens();
      this.AddScreen(screen);
    }

    public void Update()
    {
      if (this.screens.Count <= 0)
        return;
      this.CurrentState.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.screens.Count <= 0)
        return;
      this.CurrentState.Draw(spriteBatch);
    }
  }
}
