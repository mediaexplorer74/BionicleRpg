
// Type: GameManager.Commands.MapCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace GameManager.Commands
{
  public class MapCommand : ICommand
  {
    private KeyState oldState;

    public void Execute(PlayerController playerController, KeyState state)
    {
      if (state == this.oldState)
        return;
      if (state == KeyState.Down)
        Player.Instance.ShowMap = !Player.Instance.ShowMap;
      this.oldState = state;
    }
  }
}
