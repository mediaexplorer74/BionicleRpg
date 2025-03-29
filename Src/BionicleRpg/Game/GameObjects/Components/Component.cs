
// Type: GameManager.GameObjects.Components.Component
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

#nullable disable
namespace GameManager.GameObjects.Components
{
  public abstract class Component
  {
    protected Component() { }
    protected Component(GameObject gameObject) : this(){ }

    private bool isEnabled = true;

    public GameObject GameObject { get; set; }

    public Transform Transform { get; set; }

    public bool IsEnabled
    {
      get => this.isEnabled;
      set
      {
        if (this.isEnabled == value)
          return;
        if (value)
          GameObject.InvokeIfImplemented((object) this, "OnEnable", (object[]) null);
        else
          GameObject.InvokeIfImplemented((object) this, "OnDisable", (object[]) null);
        this.isEnabled = value;
      }
    }

        public T AddComponent<T>() where T : Component
        {
            return this.GameObject.AddComponent<T>();
        }

        public bool RemoveComponent(Component component) => this.GameObject.RemoveComponent(component);

    public T GetComponent<T>() where T : Component => this.GameObject.GetComponent<T>();

    public T[] GetComponents<T>() where T : Component => this.GameObject.GetComponents<T>();
  
  }
}
