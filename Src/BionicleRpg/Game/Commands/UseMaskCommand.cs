
// Type: GameManager.Commands.UseMaskCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.UI;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace GameManager.Commands
{
  public class UseMaskCommand : ICommand
  {
    private IconType imageType;
    private Cooldown cooldown;

    public UseMaskCommand(IconType imageType, Vector2 position)
    {
      this.imageType = imageType;
      this.cooldown = new Cooldown(position, 0.5f);
    }

    public void Execute(PlayerController playerController, KeyState state)
    {
      if (Player.Instance.MaskComponent == null)
        return;
      if (state == KeyState.Down && !Player.Instance.MaskComponent.PowerActive)
      {
        Player.Instance.MaskComponent.ActivateMaskPower();
        this.cooldown.StartCooldown(this.imageType);
      }
      if (state != KeyState.Up || !Player.Instance.MaskComponent.PowerActive)
        return;
      Player.Instance.MaskComponent.DeactivateMaskPower();
    }
  }
}
