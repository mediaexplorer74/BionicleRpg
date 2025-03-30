
// Type: GameManager.UI.QuestDisplay
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using GameManager.Quests;
using Microsoft.Xna.Framework;


namespace GameManager.UI
{
  public class QuestDisplay : UIComponent
  {
    private Vector2 startTextPosition = new Vector2(Game1.ScreenSize.X - /*315f*/175f, 90f);
    private Text[] questNames;
    private string questProgress;

    public QuestDisplay()
    {
        this.questNames = new Text[6];
    }

    public QuestDisplay(GameObject gameObject) : this()
    { }

    public void UpdateProgress(Quest quest)
    {
      int index = Quest.Quests.IndexOf(quest);
      if (this.questNames[index] == null)
        return;
      switch (quest)
      {
        case ClearDungeonQuest _:
          if (quest.Complete)
          {
            int num = 1;
            string str1 = num.ToString();
            num = 1;
            string str2 = num.ToString();
            this.questProgress = " " + str1 + " / " + str2;
            break;
          }
          break;
        case KillEnemiesQuest killEnemiesQuest:
          this.questProgress = " " + killEnemiesQuest.KilledCount.ToString()
                        + " / " + killEnemiesQuest.ToKill.ToString();
          break;
        case CollectBambooQuest collectBambooQuest:
          this.questProgress = " " + collectBambooQuest.Collected.ToString()
                        + " / " + collectBambooQuest.ToGet.ToString();
          break;
        case CollectHarakekeQuest collectHarakekeQuest:
          this.questProgress = " " + collectHarakekeQuest.Collected.ToString()
                        + " / " + collectHarakekeQuest.ToGet.ToString();
          break;
        case CollectOreQuest collectOreQuest:
          this.questProgress = " " + collectOreQuest.Collected.ToString() 
                        + " / " + collectOreQuest.ToGet.ToString();
          break;
        case CollectWoodQuest collectWoodQuest:
          this.questProgress = " " + collectWoodQuest.Collected.ToString()
                        + " / " + collectWoodQuest.ToGet.ToString();
          break;
      }
      this.questNames[index].TextString = quest.Name + this.questProgress;
    }

    public void UpdateSideQuestList()
    {
      for (int index = 0; index < Quest.Quests.Count; ++index)
      {
        if (this.questNames[index] == null)
        {
          if (Quest.Quests[index].IsMainQuest)
            break;
          switch (Quest.Quests[index])
          {
            case ClearDungeonQuest _:
              this.questProgress = " " + 0.ToString() + " / " + 1.ToString();
              break;
            case KillEnemiesQuest _:
              KillEnemiesQuest quest1 = (KillEnemiesQuest) Quest.Quests[index];
              this.questProgress = " " + 0.ToString() + " / " + quest1.ToKill.ToString();
              break;
            case CollectBambooQuest _:
              CollectBambooQuest quest2 = (CollectBambooQuest) Quest.Quests[index];
              this.questProgress = " " + quest2.Collected.ToString() + " / " + quest2.ToGet.ToString();
              break;
            case CollectHarakekeQuest _:
              CollectHarakekeQuest quest3 = (CollectHarakekeQuest) Quest.Quests[index];
              this.questProgress = " " + quest3.Collected.ToString() + " / " + quest3.ToGet.ToString();
              break;
            case CollectOreQuest _:
              CollectOreQuest quest4 = (CollectOreQuest) Quest.Quests[index];
              this.questProgress = " " + quest4.Collected.ToString() + " / " + quest4.ToGet.ToString();
              break;
            case CollectWoodQuest _:
              CollectWoodQuest quest5 = (CollectWoodQuest) Quest.Quests[index];
              this.questProgress = " " + quest5.Collected.ToString() + " / " + quest5.ToGet.ToString();
              break;
          }
          Text text = new Text(
              Quest.Quests[index].Name + this.questProgress, 
              this.CalculateTextPos(index), 
              /*Color.White*/Color.LightYellow, 
              /*0.75f*/0.45f, 
              TextAlignment.Right);
          text.UIStateAssign = UIStateAssign.Gameplay;
          text.IsShowing = true;
          this.questNames[index] = text;
        }
        else if (Quest.Quests[index].Complete)
          this.questNames[index] = (Text) null;
      }
    }

    public void UpdateMainQuestList()
    {
      for (int index = 0; index < Quest.Quests.Count; ++index)
      {
        if (Quest.Quests[index].IsMainQuest)
        {
          Text text = new Text(Quest.Quests[index].Name, 
              new Vector2(Game1.ScreenSize.X - /*100f*/50f, 200f), /*Color.White*/Color.LightGreen, 
              /*1f*/0.55f, 
              TextAlignment.Left);
          text.UIStateAssign = UIStateAssign.Gameplay;
          text.IsShowing = true;
          this.questNames[index] = text;
        }
      }
    }

    public Vector2 CalculateTextPos(int currentInt)
    {
      Vector2 startTextPosition = this.startTextPosition;
      if (currentInt == 0)
        return this.startTextPosition;
      startTextPosition.Y += 35f;
      this.startTextPosition = startTextPosition;
      return startTextPosition;
    }
  }
}
