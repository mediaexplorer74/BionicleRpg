
// Type: GameManager.GameObjects.Components.Lighting.VisibilityProvider
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.Map;


namespace GameManager.GameObjects.Components.Lighting
{
  public class VisibilityProvider : Component
  {
    private const int visionRange = 20;
    private Tilemap.Tile oldTile;

    public VisibilityProvider() 
    { }

    public VisibilityProvider(GameObject gameObject) : this()//: base(gameObject) 
    { }

    public void Update()
    {
      Tilemap.Tile tile = Tilemap.Instance.GetTile(this.Transform.Position);
      if (tile == null)
        return;
      this.UpdateLineOfSight(tile);
    }

    private void UpdateLineOfSight(Tilemap.Tile newTile)
    {
      if (newTile != this.oldTile)
      {
        Vector2Int tilePos;
        if (this.oldTile != null)
        {
          int x = this.oldTile.TilePos.X;
          tilePos = this.oldTile.TilePos;
          int y = tilePos.Y;
          LineOfSight.UpdateLOS(x, y, 20, false, this);
        }
        tilePos = newTile.TilePos;
        int x1 = tilePos.X;
        tilePos = newTile.TilePos;
        int y1 = tilePos.Y;
        LineOfSight.UpdateLOS(x1, y1, 20, true, this);
      }
      this.oldTile = newTile;
    }

    public void OnDisable()
    {
      if (this.oldTile == null)
        return;
      LineOfSight.UpdateLOS(this.oldTile.TilePos.X, this.oldTile.TilePos.Y, 20, false, this);
      this.oldTile = (Tilemap.Tile) null;
    }
  }
}
