
// Type: GameManager.Commands.MapZoomCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace GameManager.Commands
{
  public class MapZoomCommand : ICommand
  {
    private readonly float zoomChange;
    private const float maxAccel = 2f;
    private const float minAccel = 0.25f;
    private float accel = 0.25f;

    public MapZoomCommand(float zoomChange) => this.zoomChange = zoomChange;

    public void Execute(PlayerController playerController, KeyState state)
    {
      if (state != KeyState.Down)
      {
        this.accel -= Glob.DeltaTime * 2f;
        if ((double) this.accel >= 0.25)
          return;
        this.accel = 0.25f;
      }
      else
      {
        if (!Player.Instance.ShowMap)
          return;
        Player.Instance.MapZoom += this.zoomChange * Glob.DeltaTime * Player.Instance.MapZoom * this.accel;
        this.accel += Glob.DeltaTime;
        if ((double) this.accel <= 2.0)
          return;
        this.accel = 2f;
      }
    }
  }
}
