
// Type: GameManager.GameObjects.Components.Items.Item
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

#nullable disable
namespace GameManager.GameObjects.Components.Items
{
  public abstract class Item : Component
  {
    public int ID { get; set; }

    public string Name { get; protected set; }

    public int Value { get; private set; }

    public void Buy()
    {
    }

    public void Sell()
    {
    }
  }
}
