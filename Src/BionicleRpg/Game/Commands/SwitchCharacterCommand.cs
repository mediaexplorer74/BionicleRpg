
// Type: GameManager.Commands.SwitchCharacterCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.UI;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace GameManager.Commands
{
  public class SwitchCharacterCommand : ICommand
  {
    private readonly IconType imageType;
    private readonly Cooldown cooldown;
    private bool playCooldown;
    private readonly Element element;

    public SwitchCharacterCommand(IconType imageType, Vector2 position, Element element)
    {
      this.imageType = imageType;
      this.element = element;
      this.cooldown = new Cooldown(position, 3f);
    }

    public void Execute(PlayerController playerController, KeyState state)
    {
      if (state != KeyState.Down || Player.Instance.ShowMap || Player.Instance.SelectedElement == this.element)
        return;
      foreach (UIComponent uiComponent in UIComponent.UIComponents)
      {
        if (uiComponent is Cooldown cooldown)
        {
          if (cooldown.Active)
          {
            this.playCooldown = false;
            return;
          }
          this.playCooldown = true;
        }
      }
      if (!this.playCooldown)
        return;
      Player.Instance.SetCharacter(this.element);
      this.cooldown.StartCooldown(this.imageType);
    }
  }
}
