
// Type: GameManager.Quests.CollectOreQuest
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.Resources;
using System;


namespace GameManager.Quests
{
  public class CollectOreQuest : Quest
  {
    private int toGet;
    private int collected;

    public int ToGet => this.toGet;

    public int Collected => this.collected;

    public CollectOreQuest(Matoran questGiver, int amount)
      : base(questGiver)
    {
      this.toGet = amount;
      Ore.OnResourceGathered += new EventHandler(this.OnCollectedResource);
      this.Name = "Collect " + this.toGet.ToString() + " ore";
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
