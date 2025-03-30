
// Type: GameManager.Animation
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer


namespace GameManager
{
  public class Animation
  {
    public const float DefaultFps = 10f;

    public string Name { get; set; }

    public float Fps { get; set; }

    public string[] SpriteNames { get; set; }

    public bool RotationApplies { get; set; }

    public Animation(string name, float fps, string[] spriteNames, bool? rotationApplies)
    {
      this.Name = name;
      this.Fps = fps;
      this.SpriteNames = spriteNames;
      if (!rotationApplies.HasValue)
        return;
      this.RotationApplies = rotationApplies.Value;
    }
  }
}
