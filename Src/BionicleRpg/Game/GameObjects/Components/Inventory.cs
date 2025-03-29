
// Type: GameManager.GameObjects.Components.Inventory
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Inventory : Component
  {
    public int WoodAmount { get; set; }

    public int BambooAmount { get; set; }

    public int ProtodermisAmount { get; set; }

    public int HarakekeAmount { get; set; }

    public Inventory()  { }

    public Inventory(GameObject gameObject) : this() { }
    }
}
