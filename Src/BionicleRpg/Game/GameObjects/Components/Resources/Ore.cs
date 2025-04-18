﻿
// Type: GameManager.GameObjects.Components.Resources.Ore
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using System;


namespace GameManager.GameObjects.Components.Resources
{
  public class Ore : Resource
  {
    public static EventHandler OnResourceGathered { get; set; }

    public Ore() 
    { }

    public Ore(GameObject gameObject) : this()
    {
        
    }

    public override void ExtractResource()
    {
      Player.Instance.InventoryComponent.ProtodermisAmount += 2;
      Ore.OnResourceGathered((object) this, (EventArgs) null);
      this.GameObject.Destroy();
    }
  }
}
