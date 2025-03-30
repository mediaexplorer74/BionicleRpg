
// Type: GameManager.UI.UIComponent
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace GameManager.UI
{
  public abstract class UIComponent
  {
    private static readonly List<UIComponent> uiComponents = new List<UIComponent>();
    protected string spriteName;
    protected Vector2 position;
    protected Vector2 origin;
    protected float scale;

    public static ReadOnlyCollection<UIComponent> UIComponents
    {
      get => UIComponent.uiComponents.AsReadOnly();
    }

    public Texture2D Sprite { get; set; }

    public bool IsShowing { get; set; }

    public IconType IconType { get; set; }

    public UIStateAssign UIStateAssign { get; set; }

    public UIComponent() => UIComponent.uiComponents.Add(this);

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
