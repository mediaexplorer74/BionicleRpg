
// Type: GameManager.GameObjects.Components.Lighting.LightEmitter
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.Map;


namespace GameManager.GameObjects.Components.Lighting
{
  public class LightEmitter : Component
  {
    private Tilemap.Tile oldTile;
    private float oldAngle;

    public LightColor Color { private get; set; }

    public int Range { private get; set; } = 1;

    public float Intensity { private get; set; } = 1f;

    public float HalfArc { private get; set; } = 180f;

    public LightEmitter() 
    { }

    public LightEmitter(GameObject gameObject) : this()
    { }

    public void Update()
    {
      Tilemap.Tile tile = Tilemap.Instance.GetTile(this.Transform.Position);
      if (tile == null)
        return;
      this.UpdateLineOfSight(tile);
    }

    private void UpdateLineOfSight(Tilemap.Tile newTile, float angle = 0.0f)
    {
      if (newTile == this.oldTile && (double) this.oldAngle == (double) angle)
        return;
      (float minAngle1, float maxAngle1) = LightEmitter.CalculateArcAngle(this.oldAngle, this.HalfArc);
      (float num1, float num2) = LightEmitter.CalculateArcAngle(angle, this.HalfArc);
      if (this.oldTile != null)
        LineOfSight.UpdateLOS(this.oldTile.TilePos.X, this.oldTile.TilePos.Y, this.Range, false, this.Color * this.Intensity, this, minAngle1, maxAngle1);
      Vector2Int tilePos = newTile.TilePos;
      int x = tilePos.X;
      tilePos = newTile.TilePos;
      int y = tilePos.Y;
      int range = this.Range;
      LightColor light = this.Color * this.Intensity;
      double minAngle2 = (double) num1;
      double maxAngle2 = (double) num2;
      LineOfSight.UpdateLOS(x, y, range, true, light, this, (float) minAngle2, (float) maxAngle2);
      this.oldAngle = angle;
      this.oldTile = newTile;
    }

    private static (float, float) CalculateArcAngle(float angle, float halfArc)
    {
      angle %= 360f;
      if ((double) angle > 180.0)
        angle -= 360f;
      return (angle - halfArc, angle + halfArc);
    }

    public void OnDisable()
    {
      if (this.oldTile == null)
        return;
      LineOfSight.UpdateLOS(this.oldTile.TilePos.X, this.oldTile.TilePos.Y, this.Range, false, this.Color * this.Intensity, this, this.oldAngle - this.HalfArc, this.oldAngle + this.HalfArc);
      this.oldTile = (Tilemap.Tile) null;
    }

    public void UpdateLightSettings(
      LightColor? color,
      int? range,
      float? intensity,
      float? halfArc)
    {
      this.IsEnabled = false;
      if (color.HasValue)
        this.Color = color.Value;
      if (range.HasValue)
        this.Range = range.Value;
      if (intensity.HasValue)
        this.Intensity = intensity.Value;
      if (halfArc.HasValue)
        this.HalfArc = halfArc.Value;
      this.IsEnabled = true;
    }
  }
}
