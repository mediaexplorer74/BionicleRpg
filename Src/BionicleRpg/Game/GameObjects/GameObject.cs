
// Type: GameManager.GameObjects.GameObject
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components;
using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;


namespace GameManager.GameObjects
{
  public class GameObject
  {
    private static readonly List<GameObject> gameObjects = new List<GameObject>();
    private bool hasInitialized;
    private readonly List<Component> components = new List<Component>();
    public static ReadOnlyCollection<GameObject> GameObjects => GameObject.gameObjects.AsReadOnly();
    public Transform Transform { get; } = new Transform();
    public ReadOnlyCollection<Component> Components => this.components.AsReadOnly();
    public bool IsActive { get; private set; } = true;

    public GameObject(bool isActive = true)
    {
      if (isActive)
        GameObject.gameObjects.Add(this);
      else
        this.IsActive = false;
    }

    public void SetActive(bool value)
    {
      if (this.IsActive == value)
        return;
      if (value)
        GameObject.gameObjects.Add(this);
      else
        GameObject.gameObjects.Remove(this);
      this.IsActive = value;
    }

    public void Awake()
    {
      this.hasInitialized = true;
      this.ComponentsInvokeIfImplemented(nameof (Awake), (object[]) null, true);
    }

    public void Start()
    {
      this.ComponentsInvokeIfImplemented(nameof (Start), (object[]) null, true);
    }

    public void Update()
    {
      if (!this.hasInitialized)
      {
        this.Awake();
        this.Start();
      }
      this.ComponentsInvokeIfImplemented(nameof (Update), (object[]) null);
    }

    public void LateUpdate()
    {
      this.ComponentsInvokeIfImplemented(nameof (LateUpdate), (object[]) null);
    }

    public void LastUpdate()
    {
      this.ComponentsInvokeIfImplemented(nameof (LastUpdate), (object[]) null);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      this.ComponentsInvokeIfImplemented(nameof (Draw), new object[1]
      {
        (object) spriteBatch
      });
    }

    public void Destroy()
    {
      this.ComponentsInvokeIfImplemented("OnDestroy", (object[]) null);
      GameObject.gameObjects.Remove(this);

      this.components.Clear();

      Tilemap.Tile tile = Tilemap.Instance.GetTile(this.Transform.Position);

      if (tile.GameObject != this)
        return;

      tile.GameObject = (GameObject) null;
      tile.Collider = (Collider) null;
      EdgeShadowTilemap.Instance.SetTileObject(tile.TilePos, false);
    }

    public T AddComponent<T>() where T : Component
    {
      Component instance;
           
      if ( !(typeof(T).GetConstructor(
             new Type[1] { typeof (GameObject) }) != (ConstructorInfo)null) )
      {
        instance = (Component)Activator.CreateInstance(typeof(T), true);
      }
      else
      { 
        instance = (Component)Activator.CreateInstance(typeof(T), (object)this);
      }

      Component component = instance;
      this.components.Add(component);
      component.GameObject = this;
      component.Transform = this.Transform;
      return (T) component;
    }

    public bool RemoveComponent(Component component) => this.components.Remove(component);

    public T GetComponent<T>() where T : Component
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T component)
          return component;
      }
      return default (T);
    }

    public T[] GetComponents<T>() where T : Component
    {
      int length = 0;
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T)
          ++length;
      }
      int num = 0;
      T[] components = new T[length];
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T component)
          components[num++] = component;
      }
      return components;
    }

    private void ComponentsInvokeIfImplemented(
      string methodName,
      object[] parameters,
      bool ignoreEnabled = false)
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (ignoreEnabled || this.components[index].IsEnabled)
          this.components[index].GetType().GetMethod(methodName,
              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.Invoke(
                  (object) this.components[index], parameters);
      }
    }

    public static void InvokeIfImplemented(object target, string methodName, object[] parameters)
    {
      target.GetType().GetMethod(methodName, 
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.Invoke(target, parameters);
    }

    public void OnCollision(Collider other)
    {
    }

    public void OnCollision(Enemy other, float damage)
    {
      double damage1 = (double) other.HealthComponent.TakeDamage(damage);
    }

    public void OnCollision(Player other, float damage)
    {
      double damage1 = (double) other.GetComponent<Health>().TakeDamage(damage);
    }

    public static void ClearAll()
    {
      foreach (GameObject gameObject in GameObject.gameObjects.ToList<GameObject>())
        gameObject.IsActive = false;

      GameObject.gameObjects.Clear();

      Collider.ClearAll();
      Enemy.ClearAll();
      Matoran.ClearAll();
    }
  }
}
