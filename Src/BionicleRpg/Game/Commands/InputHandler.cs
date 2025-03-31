
// Type: GameManager.Commands.InputHandler
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Database;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.GameObjects.UI;
using GameManager.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SharpDX.MediaFoundation;
using System.Collections.Generic;


namespace GameManager.Commands
{
  public class InputHandler
  {
    private float oldPosX = 0;
    private float oldPosY = 0;
        
    private readonly Dictionary<Keys, ICommand> keyBinds 
        = new Dictionary<Keys, ICommand>();

    private readonly AttackCommand attackCommand;
    private readonly ElementalAttackCommand elementalAttackCommand;

    public static InputHandler Instance { get; } = new InputHandler();

    TouchCollection OldTouchState = TouchPanel.GetState();

    private InputHandler()
    {
      this.attackCommand = new AttackCommand();

      this.elementalAttackCommand = new ElementalAttackCommand
      (
          new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 386.0), 
          Game1.ScreenSize.Y - 100f)
      );

      this.keyBinds.Add(Keys.D, (ICommand) new MoveCommand(Vector2.UnitX));
      this.keyBinds.Add(Keys.A, (ICommand) new MoveCommand(-Vector2.UnitX));
      this.keyBinds.Add(Keys.W, (ICommand) new MoveCommand(-Vector2.UnitY));
      this.keyBinds.Add(Keys.S, (ICommand) new MoveCommand(Vector2.UnitY));

      this.keyBinds.Add(Keys.D1, 
          (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 175.0), 
          Game1.ScreenSize.Y - 100f), 
          Element.Air));

      this.keyBinds.Add(Keys.D2, 
          (ICommand) new SwitchCharacterCommand
          (
          IconType.ElementIcon, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 110.0), 
          Game1.ScreenSize.Y - 100f), 
          Element.Fire)
          );

      this.keyBinds.Add(Keys.D3, 
          (ICommand) new SwitchCharacterCommand(IconType.ElementIcon,
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 39.0),
          Game1.ScreenSize.Y - 100f), 
          Element.Earth));

      this.keyBinds.Add(Keys.D4, 
          (ICommand) new SwitchCharacterCommand(IconType.ElementIcon,
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 37.0), 
          Game1.ScreenSize.Y - 100f), 
          Element.Water));

      this.keyBinds.Add(Keys.D5, 
          (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 102.0), 
          Game1.ScreenSize.Y - 100f), 
          Element.Stone));

      this.keyBinds.Add(Keys.D6, 
          (ICommand) new SwitchCharacterCommand(IconType.ElementIcon, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 172.0), 
          Game1.ScreenSize.Y - 100f), 
          Element.Ice));

      this.keyBinds.Add(Keys.Up, (ICommand) new MapZoomCommand(1f));
      this.keyBinds.Add(Keys.Down, (ICommand) new MapZoomCommand(-1f));

      this.keyBinds.Add(Keys.LeftShift, 
          (ICommand) new UseMaskCommand(IconType.Mask, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 271.0), 
          Game1.ScreenSize.Y - 100f)));

      this.keyBinds.Add(Keys.F, (ICommand) new InteractCommand());
      this.keyBinds.Add(Keys.M, (ICommand) new MapCommand());
    }

    public void ExecuteF()
    {
        //RnD: remark it if keyboard control state unstable
        //if (state == this.oldState)
        // return;

        if (Player.Instance.QuestGiver != null)
        {
            if (Player.Instance.QuestGiver.Quest.Complete)
                Quest.CompleteQuest(Player.Instance.QuestGiver.Quest);
            else
                Player.Instance.QuestGiver.AcceptQuest();
        }
        else
        {
            Tilemap.Tile tile = Tilemap.Instance.GetTile(
                Player.Instance.Transform.Position);

            if (tile == null)
                return;
            switch (tile.Type)
            {
                case TileType.DungeonEntrance:
                    tile.GameObject.GetComponent<DungeonEntrance>().Enter();
                    break;

                case TileType.DungeonExit:
                    //RnD: StateManager using
                    //StateManager.Instance.RemoveScreen();
                    tile.GameObject.GetComponent<DungeonExit>().Exit();
                    break;

                case TileType.Savestone:
                    DatabaseManager.Instance.SaveGame();
                    break;

            }
        }
    }//ExecuteF

    public void Execute(PlayerController playerController)
    {
        TouchCollection TouchState = TouchPanel.GetState();

        /*if (TouchState.Count > 0)
        {
            oldPosX = TouchState[0].Position.X;
            oldPosY = TouchState[0].Position.Y;
        }*/
        if (OldTouchState.Count > 0)
        {
            oldPosX = OldTouchState[0].Position.X;
            oldPosY = OldTouchState[0].Position.Y;
        }

        if (TouchState.Count > 0)
        {
            // ++++++++++++++++++++++++++++++++++++++++
            if (TouchState[0].Position.X > oldPosX)
            {
                //Move right
                playerController.GetComponent<Movement>().Move(Vector2.UnitX);
            }

            if (TouchState[0].Position.X < oldPosX)
            {
                //Move left
                playerController.GetComponent<Movement>().Move(-Vector2.UnitX);
            }

            if (TouchState[0].Position.Y > oldPosY)
            {
                //Move down
                playerController.GetComponent<Movement>().Move(Vector2.UnitY);
            }

            if (TouchState[0].Position.Y < oldPosY)
            {
                //Move up
                playerController.GetComponent<Movement>().Move(-Vector2.UnitY);
            }


            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                // ****************
            Rectangle touchrectangle = new Rectangle
                (
                    (int)(TouchState[0].Position.X / Game1.screenScale.X),
                    (int)(TouchState[0].Position.Y / Game1.screenScale.Y),
                    1,
                    1
                );
            // ******************
                        
            // 1
            Rectangle ZoneMask1 = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 - 175.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneMask1) && OldTouchState.Count == 0)
            {
                  
                var SW = new SwitchCharacterCommand(IconType.ElementIcon,
                    new Vector2((float)((double)Game1.ScreenSize.X / 2.0 - 175.0),
                    Game1.ScreenSize.Y - 100f),
                    Element.Air);
                        
                    SW.Execute2();
            }

            // 2
            Rectangle ZoneMask2 = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 - 110.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneMask2) && OldTouchState.Count == 0)
            {
                
                var SW = new SwitchCharacterCommand(IconType.ElementIcon,
                    new Vector2((float)((double)Game1.ScreenSize.X / 2.0 - 110.0),
                    Game1.ScreenSize.Y - 100f),
                    Element.Fire);

                SW.Execute2();
            }

            //3
            Rectangle ZoneMask3 = new Rectangle
            (
            (int)((Game1.ScreenSize.X / 2.0 -39.0)),
            (int)((Game1.ScreenSize.Y - 100f)),
            44,
            44
            );

            if (touchrectangle.Intersects(ZoneMask3) && OldTouchState.Count == 0)
            {
                  
                var SW = new SwitchCharacterCommand(IconType.ElementIcon,
                    new Vector2((float)((double)Game1.ScreenSize.X / 2.0 - 39.0),
                    Game1.ScreenSize.Y - 100f),
                    Element.Earth);

                SW.Execute2();
            }

            // 4
            Rectangle ZoneMask4 = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 + 37.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneMask4) && OldTouchState.Count == 0)
            {

                var SW = new SwitchCharacterCommand(IconType.ElementIcon,
                    new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 37.0),
                    Game1.ScreenSize.Y - 100f),
                    Element.Water);

                SW.Execute2();
            }

            // 5
            Rectangle ZoneMask5 = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 + 102.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneMask5) && OldTouchState.Count == 0)
            {

                var SW = new SwitchCharacterCommand(IconType.ElementIcon,
                        new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 102.0),
                        Game1.ScreenSize.Y - 100f),
                        Element.Stone);

                SW.Execute2();
            }

            //6
            Rectangle ZoneMask6 = new Rectangle
            (
            (int)((Game1.ScreenSize.X / 2.0 + 172.0)),
            (int)((Game1.ScreenSize.Y - 100f)),
            44,
            44
            );

            if (touchrectangle.Intersects(ZoneMask6) && OldTouchState.Count == 0)
            {

                var SW = new SwitchCharacterCommand(IconType.ElementIcon,
                        new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 172.0),
                        Game1.ScreenSize.Y - 100f),
                        Element.Ice);

                SW.Execute2();
            }
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


            Rectangle ZoneF = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 - 271.0)),
                (int)((Game1.ScreenSize.Y - 100f)), 
                44,
                44
            );


            if (touchrectangle.Intersects(ZoneF)/* && OldTouchState.Count == 0*/)
            {
                //emulate "F" key pressing
                ExecuteF();
            }

            // +++++++++++++++++
            // L.MouseButton Click emulation

            Rectangle ZoneLClick = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 + 346.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneLClick) && OldTouchState.Count == 0)
            {
                var AC = new AttackCommand();

                AC.Execute(playerController);
            }
            // +++++++++++++++++
            // R. Mouse Button Click emulation

            Rectangle ZoneRClick = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 + 270.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneRClick) && OldTouchState.Count == 0)
            {
                /*this.elementalAttackCommand*/
                var EAC = new ElementalAttackCommand
                (
                    new Vector2((float)((double)Game1.ScreenSize.X / 2.0 + 270.0),
                    Game1.ScreenSize.Y - 100f)
                );

                EAC.Execute(playerController);
            }

            // +++++++++++++++++++++++++++++++++++++++++++
            // LShift
            Rectangle ZoneLShift = new Rectangle
            (
                (int)((Game1.ScreenSize.X / 2.0 - 271.0)),
                (int)((Game1.ScreenSize.Y - 100f)),
                44,
                44
            );

            if (touchrectangle.Intersects(ZoneLShift) && OldTouchState.Count == 0)
            {

                var UMC = new UseMaskCommand(IconType.Mask,
                    new Vector2((float)((double)Game1.ScreenSize.X / 2.0 - 271.0),
                    Game1.ScreenSize.Y - 100f));

                UMC.Execute(playerController, KeyState.Down);

                this.OldTouchState = TouchState;
                return;
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        }

        this.OldTouchState = TouchState;
        // ******************************************************************

      KeyboardState kbdState = Keyboard.GetState();

      foreach (Keys key in this.keyBinds.Keys)
      {
        this.keyBinds[key].Execute(playerController, kbdState[key]);
      }

   // ***********************************************************************************

      MouseState mouseState = Mouse.GetState();

      if (mouseState.LeftButton == ButtonState.Pressed)
      {
        //RnD: remark it for disable near attack
        this.attackCommand.Execute(playerController);
      }

      //RnD
      //mouseState = Mouse.GetState();

      if (mouseState.RightButton != ButtonState.Pressed)
        return;

      this.elementalAttackCommand.Execute(playerController);
    }
  }
}
