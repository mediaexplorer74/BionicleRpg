
// Type: GameManager.GameObjects.Components.NpcComponents.Seeker
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.ObjectModel;
using System.Threading;

#nullable disable
namespace GameManager.GameObjects.Components.NpcComponents
{
  public class Seeker : Component
  {
    //private Thread pathfindingThread;
    private Vector2 origin;
    private Vector2 goal;
    private float fleeDist = -1f;
    private Seeker.OnPathfindCompleteDelegate callback;

    public Seeker()// : base()
    {
        // Initialize with gameObject if needed
        //Seeker.Instance = this;
    }
    public Seeker(GameObject gameObject) : this()
    {
        this.GameObject = gameObject;
    }

    public void StartPath(Vector2 origin, Vector2 goal, 
                       Seeker.OnPathfindCompleteDelegate callback)
    {
      //Thread pathfindingThread = this.pathfindingThread;
     // if ((pathfindingThread != null ? (pathfindingThread.IsAlive ? 1 : 0) : 0) != 0)
     //  return;
      this.origin = origin;
      this.goal = goal;
      this.fleeDist = -1f;
      this.callback = callback;

        //Plan A
        //this.pathfindingThread = new Thread(new ParameterizedThreadStart(Seeker.ThreadUpdate))
        //{
        //  IsBackground = true
        //};
        //this.pathfindingThread.Start((object) this);

            // Plan B
        Seeker.ThreadUpdate((object)this);
    }

    public void StartPath( Vector2 origin, float fleeDist,  Seeker.OnPathfindCompleteDelegate callback)
    {
      //Thread pathfindingThread = this.pathfindingThread;
      //if ((pathfindingThread != null ? (pathfindingThread.IsAlive ? 1 : 0) : 0) != 0)
      //  return;
      this.origin = origin;
      this.fleeDist = fleeDist;
      this.callback = callback;
            //this.pathfindingThread = new Thread(new ParameterizedThreadStart(Seeker.ThreadUpdate))
            //{
            //  IsBackground = true
            //};
            //this.pathfindingThread.Start((object) this);
            Seeker.ThreadUpdate((object)this);
    }

    private static void ThreadUpdate(object obj)
    {
      Seeker seeker = (Seeker) obj;
      try
      {
        ReadOnlyCollection<Vector2Int> path = 
              (double) seeker.fleeDist != -1.0 
              ? Pathfinder.Search(seeker.origin, seeker.fleeDist) 
              : Pathfinder.Search(seeker.origin, seeker.goal);

        seeker.callback(path);
      }
      catch (Exception ex)
      {
        seeker.callback((ReadOnlyCollection<Vector2Int>) null);
      }
    }

    public delegate void OnPathfindCompleteDelegate(ReadOnlyCollection<Vector2Int> path);
  }
}
