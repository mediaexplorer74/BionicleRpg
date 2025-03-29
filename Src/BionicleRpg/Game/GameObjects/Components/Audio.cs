
// Type: GameManager.GameObjects.Components.Audio
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace GameManager.GameObjects.Components
{
  public class Audio : Component
  {
    private readonly ContentManager content = Glob.Content;
    private SoundEffect soundEffect;

    public Audio() { }

    public Audio(GameObject gameObject) : this() { }
    
    public void Play(string sound)
    {
      this.soundEffect = this.content.Load<SoundEffect>(sound);
      this.soundEffect?.Play();
    }
  }
}
