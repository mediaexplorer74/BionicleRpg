
// Type: GameManager.Commands.AttackCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework.Input;
using System;


namespace GameManager.Commands
{
    public class AttackCommand : ICommand
    {
        public void Execute(PlayerController playerController, KeyState state)
        {
            playerController.GetComponent<Combat>().UseWeapon();
        }

        public void Execute(PlayerController playerController)
        {
            //RnD
            //throw new NotImplementedException();
            playerController.GetComponent<Combat>().UseWeapon();
        }
    }
}
