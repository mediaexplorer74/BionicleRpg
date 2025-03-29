
// Type: GameManager.ObjectPool.ObjectPool
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Factories;
using GameManager.GameObjects;
using GameManager.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace GameManager.ObjectPool
{
  public abstract class ObjectPool
  {
    private readonly Stack<GameObject>[][] inactive = Enumerable.Range(0, Enum.GetValues(typeof (Element)).Length).Select<int, Stack<GameObject>[]>((Func<int, Stack<GameObject>[]>) (_ => Enumerable.Range(0, Enum.GetValues(typeof (AttackType)).Length).Select<int, Stack<GameObject>>((Func<int, Stack<GameObject>>) (_ => new Stack<GameObject>())).ToArray<Stack<GameObject>>())).ToArray<Stack<GameObject>[]>();

    public GameObject GetObject(Element element, AttackType attackType, GameObject Owner)
    {
      Stack<GameObject> gameObjectStack = this.inactive[(int) element][(int) attackType];
      if (gameObjectStack.Count == 0)
        return this.CreateObject(element, attackType);
      GameObject gameObject = gameObjectStack.Pop();
      gameObject.SetActive(true);
      this.ObjectRetrieved(gameObject, Owner);
      return gameObject;
    }

    public void ReleaseObject(Element element, AttackType attackType, GameObject gameObject)
    {
      this.inactive[(int) element][(int) attackType].Push(gameObject);
      gameObject.SetActive(false);
      this.CleanUp(gameObject);
    }

    public virtual GameObject CreateObject() => throw new NotImplementedException();

    public virtual GameObject CreateObject(Element element, AttackType attackType)
    {
      throw new NotImplementedException();
    }

    protected abstract void ObjectRetrieved(GameObject gameObject, GameObject owner);

    protected abstract void CleanUp(GameObject gameObject);
  }
}
