
// Type: GameManager.Database.SavegameData
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using Microsoft.Xna.Framework;


namespace GameManager.Database
{
  public class SavegameData(GameObject gameObject)
  {
     public SavegameData() : this(null)
     {
     }

    public int ID { get; set; }

    public int WorldSeed { get; set; }

    public float Health { get; set; }

    public float Energy { get; set; }

    public Vector2 WorldPosition { get; set; }

    public GameObject GameObject { get; } = gameObject;
    }
}
