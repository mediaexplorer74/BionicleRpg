
// Type: GameManager.Quests.CollectHarakekeQuest
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.Resources;
using System;

#nullable disable
namespace GameManager.Quests
{
  public class CollectHarakekeQuest : Quest
  {
    private int toGet;
    private int collected;

    public int ToGet => this.toGet;

    public int Collected => this.collected;

    public CollectHarakekeQuest(Matoran questGiver, int amount)
      : base(questGiver)
    {
      this.toGet = amount;
      Harakeke.OnResourceGathered += new EventHandler(this.OnCollectedResource);
      this.Name = "Collect " + this.toGet.ToString() + " harakeke";
    }

    public override void CompletionCheck()
    {
      if (this.collected < this.toGet)
        return;
      this.Complete = true;
    }

    private void OnCollectedResource(object sender, EventArgs e)
    {
      if (!this.IsAccepted || this.Complete)
        return;
      ++this.collected;
      Quest.uiQuestDisplay.UpdateProgress((Quest) this);
    }
  }
}
