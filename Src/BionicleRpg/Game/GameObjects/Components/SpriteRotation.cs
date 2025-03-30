
// Type: GameManager.GameObjects.Components.SpriteRotation
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer


namespace GameManager.GameObjects.Components
{
  public class SpriteRotation : Component
  {
    public int RotationId { get; private set; } = 1;

    public SpriteRotation()  
    { }

    public SpriteRotation(GameObject gameObject) : this() 
    { }

    public void Update()
    {
        this.FindRotationId();
    }

    private void FindRotationId()
    {
      float worldDirection = this.GameObject.Transform.WorldDirection;
      if ((double) worldDirection >= -22.5 && (double) worldDirection < 22.5)
        this.RotationId = 1;
      else if ((double) worldDirection >= -67.5 && (double) worldDirection < -22.5)
        this.RotationId = 2;
      else if ((double) worldDirection >= -112.5 && (double) worldDirection < -67.5)
        this.RotationId = 3;
      else if ((double) worldDirection >= -157.5 && (double) worldDirection < -112.5)
        this.RotationId = 4;
      else if ((double) worldDirection >= 112.5 && (double) worldDirection < 157.5)
        this.RotationId = 6;
      else if ((double) worldDirection >= 67.5 && (double) worldDirection < 112.5)
        this.RotationId = 7;
      else if ((double) worldDirection >= 22.5 && (double) worldDirection < 67.5)
        this.RotationId = 8;
      else
        this.RotationId = 5;
    }
  }
}
