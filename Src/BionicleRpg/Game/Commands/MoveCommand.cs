
// Type: GameManager.Commands.MoveCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace GameManager.Commands
{
  public class MoveCommand : ICommand
  {
    private Vector2 velocity;

    public MoveCommand(Vector2 velocity) => this.velocity = velocity;

    public void Execute(PlayerController playerController, KeyState state)
    {
      if (state != KeyState.Down)
        return;
      playerController.GetComponent<Movement>().Move(this.velocity);
    }
  }
}
