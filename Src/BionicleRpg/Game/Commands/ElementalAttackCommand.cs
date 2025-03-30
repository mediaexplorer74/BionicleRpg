
// Type: GameManager.Commands.ElementalAttackCommand
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
    public class ElementalAttackCommand : ICommand
    {
        private readonly Cooldown cooldown;

        public ElementalAttackCommand(Vector2 position)
        {
            this.cooldown = new Cooldown(position, 0.4f);
        }

        public void Execute(PlayerController playerController)
        {
            playerController.GetComponent<Combat>().UseElement();
            this.cooldown.StartCooldown(IconType.ElementPower);
        }

        public void Execute(PlayerController playerController, KeyState state)
        {
            // Implement the logic for handling the KeyState if needed
            Execute(playerController);
        }
    }
}
