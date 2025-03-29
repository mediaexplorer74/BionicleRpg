
// Type: GameManager.IListExtensions
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public static class IListExtensions
  {
    public static T GetRandomElement<T>(this IList<T> collection, Random random)
    {
      return collection.Count <= 0 ? default (T) : collection[random.Next(0, collection.Count)];
    }
  }
}
