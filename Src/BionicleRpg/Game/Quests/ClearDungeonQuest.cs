
// Type: GameManager.Quests.ClearDungeonQuest
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.NpcComponents;
using GameManager.States;

#nullable disable
namespace GameManager.Quests
{
  public class ClearDungeonQuest : Quest
  {
    public int DungeonSeed { get; }

    public ClearDungeonQuest(Matoran questGiver, int dungeonSeed)
      : base(questGiver)
    {
      this.DungeonSeed = dungeonSeed;
      this.Name = "Clear the marked dungeon";
    }

    public override void CompletionCheck()
    {
      if (!(StateManager.Instance.CurrentState is DungeonState currentState) || currentState.Seed != this.DungeonSeed || currentState.RemainingEnemyCount > 0)
        return;
      this.Complete = true;
      Quest.uiQuestDisplay.UpdateProgress((Quest) this);
    }
  }
}
