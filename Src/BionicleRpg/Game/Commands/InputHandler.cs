
// Type: GameManager.Commands.InputHandler
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace GameManager.Commands
{
  public class InputHandler
  {
    private readonly Dictionary<Keys, ICommand> keyBinds = new Dictionary<Keys, ICommand>();
    private readonly AttackCommand attackCommand;
    private readonly ElementalAttackCommand elementalAttackCommand;

    public static InputHandler Instance { get; } = new InputHandler();

    private InputHandler()
    {
      this.attackCommand = new AttackCommand();

      this.elementalAttackCommand = new ElementalAttackCommand(
          new Vector2(/*898f*/(float)((double)Game1.ScreenSize.X / 2.0 +386.0), Game1.ScreenSize.Y - 100f)
          );

      this.keyBinds.Add(Keys.D, (ICommand) new MoveCommand(Vector2.UnitX));
      this.keyBinds.Add(Keys.A, (ICommand) new MoveCommand(-Vector2.UnitX));
      this.keyBinds.Add(Keys.W, (ICommand) new MoveCommand(-Vector2.UnitY));
      this.keyBinds.Add(Keys.S, (ICommand) new MoveCommand(Vector2.UnitY));

      this.keyBinds.Add(Keys.D1, (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 175.0), Game1.ScreenSize.Y - 100f), Element.Air));

      this.keyBinds.Add(Keys.D2, (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 110.0), Game1.ScreenSize.Y - 100f), Element.Fire));

      this.keyBinds.Add(Keys.D3, (ICommand) new SwitchCharacterCommand(IconType.ElementIcon,
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 39.0), Game1.ScreenSize.Y - 100f), Element.Earth));

      this.keyBinds.Add(Keys.D4, (ICommand) new SwitchCharacterCommand(IconType.ElementIcon,
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 37.0), Game1.ScreenSize.Y - 100f), Element.Water));

      this.keyBinds.Add(Keys.D5, (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 102.0), Game1.ScreenSize.Y - 102f), Element.Stone));

      this.keyBinds.Add(Keys.D6, (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 172.0), Game1.ScreenSize.Y - 100f), Element.Ice));

      this.keyBinds.Add(Keys.Up, (ICommand) new MapZoomCommand(1f));
      this.keyBinds.Add(Keys.Down, (ICommand) new MapZoomCommand(-1f));

      this.keyBinds.Add(Keys.LeftShift, (ICommand) new UseMaskCommand(IconType.Mask, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 271.0), Game1.ScreenSize.Y - 100f)));

      this.keyBinds.Add(Keys.F, (ICommand) new InteractCommand());
      this.keyBinds.Add(Keys.M, (ICommand) new MapCommand());
    }

    public void Execute(PlayerController playerController)
    {
      KeyboardState state1 = Keyboard.GetState();

        foreach (Keys key in this.keyBinds.Keys)
        {
            this.keyBinds[key].Execute(playerController, state1[key]);
        }

      MouseState state2 = Mouse.GetState();

      if (state2.LeftButton == ButtonState.Pressed)
      {
        //RnD: remark it for disable near attack
        this.attackCommand.Execute(playerController);
      }

      state2 = Mouse.GetState();

      if (state2.RightButton != ButtonState.Pressed)
        return;

      this.elementalAttackCommand.Execute(playerController);
    }
  }
}
