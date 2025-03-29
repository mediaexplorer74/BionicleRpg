
// Type: GameManager.Builders.BuilderDirector
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;

#nullable disable
namespace GameManager.Builders
{
  public class BuilderDirector
  {
    public GameObject ConstructPlayer()
    {
      PlayerBuilder.Instance.BuildGameObject();
      return PlayerBuilder.Instance.GetResult();
    }
  }
}
