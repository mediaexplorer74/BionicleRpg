
// Type: GameManager.Layers.LayerManager
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using System;


namespace GameManager.Layers
{
  public class LayerManager
  {
    private readonly float[] sortingLayerDepths;

    public static LayerManager Instance { get; } = new LayerManager();

    private LayerManager()
    {
      int length = Enum.GetValues(typeof (SortingLayer)).Length;
      float num = 1f / (float) length;
      this.sortingLayerDepths = new float[length];
      for (int index = 0; index < length; ++index)
        this.sortingLayerDepths[index] = num * (float) (index + 1);
    }

    public float GetLayerDepth(SortingLayer sortingLayer, int orderInLayer)
    {
      return this.sortingLayerDepths[(int) sortingLayer] + (float) (orderInLayer / 100000);
    }
  }
}
