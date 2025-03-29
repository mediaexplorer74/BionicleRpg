
// Type: GameManager.States.LoadingState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.UI;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.States
{
  public class LoadingState : IState
  {
    private IState nextScreen;

    public LoadingState(IState nextScreen) => this.nextScreen = nextScreen;

    public void Enter() => UIManager.Instance.ShowUIComponent(UIStateAssign.Loading, true);

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void Update() => StateManager.Instance.ChangeScreen(this.nextScreen);

    public void Exit() => UIManager.Instance.ShowUIComponent(UIStateAssign.Loading, false);
  }
}
