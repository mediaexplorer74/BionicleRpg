
// Type: GameManager.States.IState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.States
{
  public interface IState
  {
    void Enter();

    void Update();

    void Draw(SpriteBatch spriteBatch);

    void Exit();
  }
}
