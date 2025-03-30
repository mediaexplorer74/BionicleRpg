
// Type: GameManager.States.OverworldState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components;
using GameManager.UI;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.States
{
  public class OverworldState : IState
  {
    public const int OverworldMapSize = 1000;
    private bool wasInitialized;

    public void Enter()
    {
      if (!this.wasInitialized)
      {
        Game1.CreateWorld(1000, 1000);
        Game1.InitGameObjects();
        this.wasInitialized = true;
      }
      UIManager.Instance.ShowUIComponent(UIStateAssign.Gameplay, true);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void Update() => EnemySpawner.Instance.Update();

    public void Exit() => UIManager.Instance.ShowUIComponent(UIStateAssign.Gameplay, false);
  }
}
