
// Type: GameManager.GameObjects.Components.Resources.Tree
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using System;

#nullable disable
namespace GameManager.GameObjects.Components.Resources
{
  public class Tree : Resource
  {
    public static EventHandler OnResourceGathered { get; set; }

    public Tree()  { }

    public Tree(GameObject gameObject) : this()
    {
        this.GameObject = gameObject;
    }

    public override void ExtractResource()
    {
      Player.Instance.InventoryComponent.WoodAmount += 2;
      Tree.OnResourceGathered((object) this, (EventArgs) null);
      this.GameObject.Destroy();
    }
  }
}
