
// Type: GameManager.Database.CharacterData
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.GameObjects.Components;


namespace GameManager.Database
{
  public class CharacterData
  {
    public int CharacterID { get; set; }

    public string Name { get; set; }

    public Element Element { get; set; }

    public AttackType CurrentWeapon { get; set; }

    public MaskType CurrentMask { get; set; }
  }
}
