
// Type: GameManager.Quests.Quest
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;

#nullable disable
namespace GameManager.Quests
{
  public abstract class Quest
  {
    private static readonly List<Type> questTypes = new List<Type>();
    private static readonly List<Quest> quests = new List<Quest>();
    private bool complete;
    protected static QuestDisplay uiQuestDisplay = new QuestDisplay();

    public bool IsMainQuest { get; private set; }

    public static ReadOnlyCollection<Quest> Quests => Quest.quests.AsReadOnly();

    public bool Complete
    {
      get => this.complete;
      set
      {
        bool complete = this.complete;
        this.complete = value;
        if (!value || complete == value)
          return;
        this.QuestGiver.SetQuestSpeechBubbleEnabled(true);
      }
    }

    public bool IsAccepted { get; private set; }

    public string Name { get; set; }

    public Matoran QuestGiver { get; }

    public Quest(Matoran questGiver) => this.QuestGiver = questGiver;

    public static void PerformCompletionChecks()
    {
      for (int index = 0; index < Quest.quests.Count; ++index)
        Quest.quests[index].CompletionCheck();
    }

    public abstract void CompletionCheck();

        public static void Initialize()
        {
            foreach (Type type 
                in typeof(Quest).GetTypeInfo().Assembly.GetTypes())
            {
                 if (typeof(Quest).IsAssignableFrom(type) && type != typeof(Quest))
                     Quest.questTypes.Add(type);
             }            
        }

    public static void AcceptQuest(Quest questToAccept)
    {
      Quest.quests.Add(questToAccept);
      questToAccept.IsAccepted = true;
      Quest.uiQuestDisplay.UpdateSideQuestList();
    }

    public static void CompleteQuest(Quest questToComplete)
    {
      questToComplete.QuestGiver.Quest = (Quest) null;
      questToComplete.QuestGiver.SetQuestSpeechBubbleEnabled(false);
      Player.Instance.QuestGiver = (Matoran) null;
      Quest.quests.Remove(questToComplete);
      Quest.uiQuestDisplay.UpdateSideQuestList();
    }

    public static Quest GenerateRandom(Matoran matoran, Vector2 worldPos)
    {
      Type randomElement = Quest.questTypes.GetRandomElement<Type>(Tilemap.Instance.Random);
      if (randomElement == typeof (ClearDungeonQuest))
      {
        Vector2Int vector2Int = Vector2Int.RoundToInt(worldPos / 50f);
        HashSet<Vector2Int> vector2IntSet = new HashSet<Vector2Int>()
        {
          vector2Int
        };
        Queue<Vector2Int> vector2IntQueue = new Queue<Vector2Int>();
        vector2IntQueue.Enqueue(vector2Int);
        while (vector2IntQueue.Count > 0)
        {
          Vector2Int origin = vector2IntQueue.Dequeue();
          for (int direction = 0; direction < 4; ++direction)
          {
            Vector2Int adjacentTilePos = Quest.GetAdjacentTilePos(origin, direction);
            if (!Tilemap.Instance.IsOutOfTileBounds(adjacentTilePos) && !vector2IntSet.Contains(adjacentTilePos))
            {
              Tilemap.Tile tile = Tilemap.Instance.Tiles[adjacentTilePos];
              if (tile.IsTraversable)
              {
                if (tile.Type == TileType.DungeonEntrance && Tilemap.Instance.Random.Next(0, 4) == 0)
                  return (Quest) new ClearDungeonQuest(matoran, tile.GameObject.GetComponent<DungeonEntrance>().Seed);
                vector2IntSet.Add(adjacentTilePos);
                vector2IntQueue.Enqueue(adjacentTilePos);
              }
            }
          }
        }
      }
      else
      {
        if (randomElement == typeof (KillEnemiesQuest))
          return (Quest) new KillEnemiesQuest(matoran, Tilemap.Instance.Random.Next(5, 20));
        if (randomElement == typeof (CollectBambooQuest))
          return (Quest) new CollectBambooQuest(matoran, Tilemap.Instance.Random.Next(5, 10));
        if (randomElement == typeof (CollectHarakekeQuest))
          return (Quest) new CollectHarakekeQuest(matoran, Tilemap.Instance.Random.Next(5, 10));
        if (randomElement == typeof (CollectWoodQuest))
          return (Quest) new CollectWoodQuest(matoran, Tilemap.Instance.Random.Next(5, 10));
        if (randomElement == typeof (CollectOreQuest))
          return (Quest) new CollectOreQuest(matoran, Tilemap.Instance.Random.Next(5, 10));
      }
      return (Quest) null;
    }

    private static Vector2Int GetAdjacentTilePos(Vector2Int origin, int direction)
    {
      switch (direction)
      {
        case 0:
          return new Vector2Int(origin.X, origin.Y - 1);
        case 1:
          return new Vector2Int(origin.X + 1, origin.Y);
        case 2:
          return new Vector2Int(origin.X, origin.Y - 1);
        case 3:
          return new Vector2Int(origin.X - 1, origin.Y);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
