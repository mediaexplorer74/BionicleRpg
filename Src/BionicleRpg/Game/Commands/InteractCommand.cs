
// Type: GameManager.Commands.InteractCommand
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Database;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.Quests;
using GameManager.States;
using Microsoft.Xna.Framework.Input;


namespace GameManager.Commands
{
  public class InteractCommand : ICommand
  {
    private KeyState oldState;

    public void Execute(PlayerController playerController, KeyState state)
    {
      //if (state == this.oldState)
      //  return;
      this.oldState = state;
      if (state != KeyState.Down)
        return;
      if (Player.Instance.QuestGiver != null)
      {
        if (Player.Instance.QuestGiver.Quest.Complete)
          Quest.CompleteQuest(Player.Instance.QuestGiver.Quest);
        else
          Player.Instance.QuestGiver.AcceptQuest();
      }
      else
      {
        Tilemap.Tile tile = Tilemap.Instance.GetTile(Player.Instance.Transform.Position);
        if (tile == null)
          return;
        switch (tile.Type)
        {
          case TileType.DungeonEntrance:
            tile.GameObject.GetComponent<DungeonEntrance>().Enter();
            break;
          case TileType.DungeonExit:
            StateManager.Instance.RemoveScreen();
            break;
          case TileType.Savestone:
            DatabaseManager.Instance.SaveGame();
            break;
        }
      }
    }
  }
}
