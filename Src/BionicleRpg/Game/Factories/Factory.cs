
// Type: GameManager.Factories.Factory
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using Microsoft.Xna.Framework.Content;
using System;

#nullable disable
namespace GameManager.Factories
{
  public abstract class Factory
  {
    public virtual GameObject Create() => throw new NotImplementedException();

    public virtual GameObject Create(Element element) => throw new NotImplementedException();

    public virtual GameObject Create(MaskType maskType) => throw new NotImplementedException();

    public virtual GameObject Create(Element element, AttackType attackType)
    {
      throw new NotImplementedException();
    }
  }
}
