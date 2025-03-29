
// Type: GameManager.GameObjects.Components.Tilemaps.EdgeShadowTilemap
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
  public class EdgeShadowTilemap : Component, IRenderable
  {
    private static readonly Color wallShadow = new Color(0.0f, 0.0f, 0.025f, 1f);
    private static readonly Color objectShadow = new Color(0.0f, 0.0f, 0.025f, 0.5f);

    public static EdgeShadowTilemap Instance { get; private set; }

    public Texture2D Texture { get; set; }

    public EdgeShadowTilemap() 
    {
        EdgeShadowTilemap.Instance = this;
    }

    public EdgeShadowTilemap(GameObject gameObject) : this()
    {
       
    }

    public void Awake()
    {
      this.Texture = new Texture2D(Glob.GraphicsDevice, Tilemap.Instance.Width, Tilemap.Instance.Height);
      Color[] data = new Color[Tilemap.Instance.Width * Tilemap.Instance.Height];
      for (int index = 0; index < data.Length; ++index)
        data[index] = Color.Transparent;
      this.Texture.SetData<Color>(data);
    }

    public void SetTileWall(Vector2Int pos, bool value)
    {
      this.SetPixel(pos.X + Tilemap.Instance.HalfWidth, pos.Y + Tilemap.Instance.HalfHeight, value ? EdgeShadowTilemap.wallShadow : Color.Transparent);
    }

    public void SetTileObject(Vector2Int pos, bool value)
    {
      this.SetPixel(pos.X + Tilemap.Instance.HalfWidth, pos.Y + Tilemap.Instance.HalfHeight, value ? EdgeShadowTilemap.objectShadow : Color.Transparent);
    }

    private void SetPixel(int x, int y, Color color)
    {
      this.Texture.SetData<Color>(0, new Rectangle?(new Rectangle(x, y, 1, 1)), new Color[1]
      {
        color
      }, 0, 1);
    }
  }
}
