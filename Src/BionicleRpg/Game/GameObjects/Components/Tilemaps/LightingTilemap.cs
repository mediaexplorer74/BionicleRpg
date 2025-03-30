
// Type: GameManager.GameObjects.Components.Tilemaps.LightingTilemap
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Renderers;
using GameManager.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D;


namespace GameManager.GameObjects.Components.Tilemaps
{
  public class LightingTilemap : Component, IRenderable
  {
    public static LightingTilemap Instance { get; private set; }

    public Texture2D Texture { get; set; }

    public LightColor Darkness { get; private set; }


    public LightingTilemap() 
    {
        LightingTilemap.Instance = this;
    }

    public LightingTilemap(GameObject gameObject) : this()
    {
      LightingTilemap.Instance = this;
      if (StateManager.Instance.CurrentState is DungeonState)
        this.Darkness = new LightColor(0.2f, 0.2f, 0.3f);
      else
        this.Darkness = new LightColor(0.9f, 0.9f, 0.8f);
    }

    public void Awake()
    {
      this.Texture = new Texture2D(Glob.GraphicsDevice, Tilemap.Instance.Width, Tilemap.Instance.Height);
      Color[] data = new Color[Tilemap.Instance.Width * Tilemap.Instance.Height];
      for (int index = 0; index < data.Length; ++index)
        data[index] = (Color) this.Darkness;
      this.Texture.SetData<Color>(data);
    }

    public void SetTileLighting(int x, int y, Color color)
    {
      this.Texture.SetData<Color>(0, new Rectangle?(new Rectangle(x + Tilemap.Instance.HalfWidth, y + Tilemap.Instance.HalfHeight, 1, 1)), new Color[1]
      {
        color
      }, 0, 1);
    }
  }
}
