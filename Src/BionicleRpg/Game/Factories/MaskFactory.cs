
// Type: GameManager.Factories.MaskFactory
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using GameManager.GameObjects.Components.Items.Masks;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Renderers;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager.Factories
{
  public class MaskFactory : Factory
  {
    private readonly Dictionary<MaskType, GameObject> masks = new Dictionary<MaskType, GameObject>();

    public static MaskFactory Instance { get; } = new MaskFactory();

    public MaskFactory()
    {
      foreach (MaskType key in Enum.GetValues(typeof (MaskType)))
      {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<SpriteRenderer>();
        switch (key)
        {
          case MaskType.Speed:
            gameObject.AddComponent<SpeedMask>();
            break;
          case MaskType.Strength:
            gameObject.AddComponent<StrengthMask>();
            break;
          case MaskType.Shielding:
            gameObject.AddComponent<ShieldMask>();
            break;
          case MaskType.Xray:
            gameObject.AddComponent<XrayMask>();
            break;
          case MaskType.WaterBreathing:
            gameObject.AddComponent<WaterBreathingMask>();
            break;
          case MaskType.Levitation:
            gameObject.AddComponent<LevitationMask>();
            break;
        }
        this.masks[key] = gameObject;
      }
    }

    public override GameObject Create(MaskType maskType)
    {
      foreach (MaskType key in Enum.GetValues(typeof (MaskType)))
        this.masks[key].SetActive(false);
      GameObject mask = this.masks[maskType];
      mask.GetComponent<SpriteRenderer>().Color = Player.Instance.ElementColor;
      mask.SetActive(true);
      return mask;
    }
  }
}
