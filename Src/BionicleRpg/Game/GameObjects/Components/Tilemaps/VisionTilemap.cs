
// Type: GameManager.GameObjects.Components.Tilemaps.VisionTilemap
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.GameObjects.Components.Tilemaps
{
  public class VisionTilemap : Component, IRenderable
  {
    public static VisionTilemap Instance { get; private set; }

    public Texture2D Texture { get; set; }

    public static Color OriginalVisionColor { get; } = Color.Black;

    public Color VisionColor { get; set; } = VisionTilemap.OriginalVisionColor;

    public VisionTilemap()
    {  
        VisionTilemap.Instance = this;
    }

    public VisionTilemap(GameObject gameObject) : this()
    {
        VisionTilemap.Instance = this;
    }

    public void Awake()
    {
      this.Texture = new Texture2D(Glob.GraphicsDevice, Tilemap.Instance.Width,
          Tilemap.Instance.Height);
      Color[] data = new Color[Tilemap.Instance.Width * Tilemap.Instance.Height];
      for (int index = 0; index < data.Length; ++index)
        data[index] = this.VisionColor;
      this.Texture.SetData<Color>(data);
    }

    public void SetTileVisible(Vector2Int pos, bool value)
    {
      this.SetPixel(pos.X + Tilemap.Instance.HalfWidth, pos.Y + Tilemap.Instance.HalfHeight, value ? Color.Transparent : this.VisionColor);
    }

    public void SetPixel(int x, int y, Color color)
    {
      this.Texture.SetData<Color>(0, new Rectangle?(new Rectangle(x, y, 1, 1)), new Color[1]
      {
        color
      }, 0, 1);
    }

    public void SetVisionColor(Color color)
    {
      this.VisionColor = color;
      Color[] data = new Color[Tilemap.Instance.Width * Tilemap.Instance.Height];
      for (int index = 0; index < data.Length; ++index)
        data[index] = this.VisionColor;
      this.Texture.SetData<Color>(data);
    }
  }
}
