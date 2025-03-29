
// Type: GameManager.GameObjects.Components.Resources.Resource
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.Items;

#nullable disable
namespace GameManager.GameObjects.Components.Resources
{
  public abstract class Resource : Item
  {
    private Collider collider;

    public abstract void ExtractResource();

    public void Start() => this.collider = this.GetComponent<Collider>();

    public void Update()
    {
    }
  }
}
