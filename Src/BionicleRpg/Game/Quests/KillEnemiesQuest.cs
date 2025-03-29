
// Type: GameManager.Quests.KillEnemiesQuest
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.NpcComponents;
using System;

#nullable disable
namespace GameManager.Quests
{
  public class KillEnemiesQuest : Quest
  {
    private readonly int toKill;
    private int killedCount;

    public int ToKill => this.toKill;

    public int KilledCount => this.killedCount;

    public KillEnemiesQuest(Matoran questGiver, int toKill)
      : base(questGiver)
    {
      this.toKill = toKill;
      Enemy.OnEnemyDeath += new EventHandler(this.OnEnemyDeath);
      this.Name = "Kill " + toKill.ToString() + " Fikou spiders";
    }

    public override void CompletionCheck()
    {
      if (this.killedCount < this.toKill)
        return;
      this.Complete = true;
    }

    private void OnEnemyDeath(object sender, EventArgs e)
    {
      if (!this.IsAccepted || this.Complete)
        return;
      ++this.killedCount;
      Quest.uiQuestDisplay.UpdateProgress((Quest) this);
    }
  }
}
