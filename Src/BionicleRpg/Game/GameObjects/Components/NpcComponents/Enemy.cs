
// Type: GameManager.GameObjects.Components.NpcComponents.Enemy
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.Components.PlayerComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace GameManager.GameObjects.Components.NpcComponents
{
  public class Enemy : Npc
  {
    private static readonly List<Enemy> enemies = new List<Enemy>();
    private double firstSawPlayer = double.NegativeInfinity;
    private Vector2 searchPos;
    private const double TimeBeforeSearch = 0.5;
    private const double SearchDurationMin = 5.0;
    private const double SearchDurationMax = 20.0;
    private double searchEndTime;
    private static Vector2 lastKnownPlayerPos;
    private static Vector2 lastKnownPlayerVelocity;
    private static double playerLastSpotted = double.NegativeInfinity;

    public static ReadOnlyCollection<Enemy> Enemies => Enemy.enemies.AsReadOnly();

    public static EventHandler OnEnemyDeath { get; set; }

    
    public Enemy() 
    {
        this.Name = "Fikou Spider";
        Enemy.enemies.Add(this);
    }

    public Enemy(GameObject gameObject) : this()
    {
        
    }

    protected override void InitializeBehaviorValues()
    {
      this.braveness = 1f;
      base.InitializeBehaviorValues();
    }

    public static void ClearAll()
    {
        Enemy.enemies.Clear();
    }

    protected override void Death()
    {
      Enemy.OnEnemyDeath((object) this, (EventArgs) null);
      Enemy.enemies.Remove(this);
      base.Death();
    }

    protected override void UpdateBehavior(GameTime gameTime)
    {
      int num1 = this.IsOutOfScreenBounds() ? 0 : (this.IsCurrentTileVisible() ? 1 : 0);
      TimeSpan totalGameTime;
      if (num1 != 0 && this.firstSawPlayer == double.NegativeInfinity)
      {
        totalGameTime = gameTime.TotalGameTime;
        this.firstSawPlayer = totalGameTime.TotalSeconds;
      }
      if (num1 != 0)
      {
        double num2 = this.firstSawPlayer + (double) Npc.GetReactionTime(this.CurrentBehaviorState) + (double) this.extraReactionTime;
        totalGameTime = gameTime.TotalGameTime;
        double totalSeconds = totalGameTime.TotalSeconds;
        if (num2 > totalSeconds)
          return;
      }
      else
        this.firstSawPlayer = double.NegativeInfinity;
      this.canSeePlayer = !this.IsOutOfScreenBounds() && this.IsCurrentTileVisible();
      if (this.canSeePlayer)
      {
        if ((double) Vector2.Distance(Player.Instance.Transform.Position, this.Transform.Position) < (double) this.attackDistance)
          this.Attack(Player.Instance.Transform.Position - this.Transform.Position);
        this.ReactToEnemy(gameTime);
        this.UpdateKnownPlayerPosition(gameTime, 0.0f);
      }
      else
      {
        switch (this.CurrentBehaviorState)
        {
          case Npc.BehaviorState.Search:
            totalGameTime = gameTime.TotalGameTime;
            if (totalGameTime.TotalSeconds - Enemy.playerLastSpotted >= 0.5)
              break;
            this.CurrentBehaviorState = (double) Game1.Random.NextFloat(0.0f, 1f) 
                            < (double) Math.Pow(this.braveness, 0.8f) 
                            ? Npc.BehaviorState.Attack
                            : Npc.BehaviorState.Retreat;
            break;
          case Npc.BehaviorState.Attack:
            totalGameTime = gameTime.TotalGameTime;
            if (totalGameTime.TotalSeconds - Enemy.playerLastSpotted <= 0.5)
              break;
            this.BeginSearch(gameTime);
            break;
        }
      }
    }

    protected override void UpdateBehaviorMovement(GameTime gameTime)
    {
      switch (this.CurrentBehaviorState)
      {
        case Npc.BehaviorState.Idle:
          this.movement.Velocity = Vector2.Zero;
          break;
        case Npc.BehaviorState.Search:
          float num1 = this.MoveTo(gameTime, this.searchPos, 1000f, 0.0f, 0.5f, false);
          if ((double) num1 > 0.5 && (double) num1 != double.PositiveInfinity)
            break;
          if (gameTime.TotalGameTime.TotalSeconds >= this.searchEndTime)
          {
            this.CurrentBehaviorState = Npc.BehaviorState.Idle;
            break;
          }
          this.GenerateNewSearchPos(gameTime);
          break;
        case Npc.BehaviorState.Attack:
          double num2 = (double) this.MoveTo(gameTime, Player.Instance.Transform.Position, 2f, 0.25f, 1f, true);
          break;
        case Npc.BehaviorState.Retreat:
          if ((double) this.MoveTo(gameTime, Player.Instance.Transform.Position, 2f, 0.0f, 0.1f, false, 10f) > 0.10000000149011612)
            break;
          this.lastPathfind = double.NegativeInfinity;
          break;
      }
    }

    private bool IsOutOfScreenBounds()
    {
      float extraVisionRange = GetExtraVisionRange();
      Vector2 vector2 = this.Transform.Position - Player.Instance.Transform.Position;
      return (double) Math.Abs(vector2.X) > (double) Game1.ScreenSize.X / 2.0 
                + (double) extraVisionRange 
                || (double) Math.Abs(vector2.Y) > (double) Game1.ScreenSize.Y / 2.0
                + (double) extraVisionRange;

      float GetExtraVisionRange()
      {
        float extraVisionRange;
        switch (this.CurrentBehaviorState)
        {
          case Npc.BehaviorState.Search:
            extraVisionRange = 100f;
            break;
          case Npc.BehaviorState.Attack:
            extraVisionRange = 1000f;
            break;
          default:
            extraVisionRange = 0.0f;
            break;
        }
        return extraVisionRange;
      }
    }

    private void UpdateKnownPlayerPosition(GameTime gameTime, float inaccuracy)
    {
      Enemy.playerLastSpotted = gameTime.TotalGameTime.TotalSeconds;
      Enemy.lastKnownPlayerPos = Player.Instance.Transform.Position + Npc.GetPosInCircle(inaccuracy) * 50f;
      Enemy.lastKnownPlayerVelocity = Player.Instance.Movement.Velocity;
    }

    private void BeginSearch(GameTime gameTime)
    {
      this.CurrentBehaviorState = Npc.BehaviorState.Search;
      this.searchEndTime = Game1.Random.NextDoubleRange(5.0, 20.0);
      this.GenerateNewSearchPos(gameTime);
    }

    private void GenerateNewSearchPos(GameTime gameTime)
    {
      float num = (float) (gameTime.TotalGameTime.TotalSeconds - Enemy.playerLastSpotted);
      this.searchPos = Enemy.lastKnownPlayerPos + Enemy.lastKnownPlayerVelocity *
                num + Npc.GetPosInCircle(Enemy.lastKnownPlayerVelocity.Length() * num) * 50f;
      this.lastPathfind = double.NegativeInfinity;
    }

    public void ReactToSound(GameTime gameTime, double chanceMultiplier, float inaccuracy)
    {
      if ((double) this.braveness < 0.5 || (double) Game1.Random.NextFloat(0.0f, 1f) >= 0.34999999403953552 * chanceMultiplier)
        return;
      this.UpdateKnownPlayerPosition(gameTime, inaccuracy);
      this.BeginSearch(gameTime);
    }
  }
}
