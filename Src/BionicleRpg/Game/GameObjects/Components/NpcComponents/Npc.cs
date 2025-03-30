
// Type: GameManager.GameObjects.Components.NpcComponents.Npc
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.Items.Weapons;
using GameManager.GameObjects.Components.Renderers;
using GameManager.GameObjects.Components.Tilemaps;
using Microsoft.Xna.Framework;
using System;
using System.Collections.ObjectModel;


namespace GameManager.GameObjects.Components.NpcComponents
{
  public abstract class Npc : Component
  {
    private SpriteRenderer spriteRenderer;
    public Element SelectedElement;
    private Combat combat;
    private Healthbar healthbar;
    private NameDisplay nameDisplay;
    protected float extraReactionTime = 0.2f;
    private const float maxWaypointDist = 5f;
    protected double lastPathfind = double.NegativeInfinity;
    protected ReadOnlyCollection<Vector2Int> path;
    private bool reachedEndOfPath;
    private int currentWaypointIndex;
    protected float braveness;
    private double nextBravenessCheck = double.NegativeInfinity;
    protected Movement movement;
    protected Animator animator;
    private Seeker seeker;
    protected int attackDistance = 100;
    protected bool canSeePlayer;

    public Rectangle CollisionBox
    {
      get
      {
        return this.spriteRenderer != null ? new Rectangle((int) ((double) this.GameObject.Transform.Position.X - (double) (this.spriteRenderer.Sprite.Width / 2) * (double) this.Transform.Scale), (int) ((double) this.GameObject.Transform.Position.Y - (double) (this.spriteRenderer.Sprite.Height / 2) * (double) this.Transform.Scale), (int) ((double) this.spriteRenderer.Sprite.Width * (double) this.Transform.Scale), (int) ((double) this.spriteRenderer.Sprite.Height * (double) this.Transform.Scale)) : new Rectangle((int) this.GameObject.Transform.Position.X, (int) this.GameObject.Transform.Position.Y, 1, 1);
      }
    }

    public Health HealthComponent { get; private set; }

    public string Name { get; protected set; } = nameof (Npc);

    protected virtual Npc.BehaviorState CurrentBehaviorState { get; set; }

    public Npc() => this.InitializeBehaviorValues();

    protected virtual void InitializeBehaviorValues()
    {
      this.extraReactionTime += Game1.Random.NextFloat(-0.03f, 0.03f);
    }

    protected virtual void Death()
    {
        this.GameObject.Destroy();
    }

    public void Awake()
    {
      this.spriteRenderer = this.GetComponent<SpriteRenderer>();
      this.movement = this.GetComponent<Movement>();
      this.combat = this.GetComponent<Combat>();
      this.seeker = this.GetComponent<Seeker>();
      this.HealthComponent = this.GetComponent<Health>();
      this.healthbar = this.GetComponent<Healthbar>();
      this.nameDisplay = this.GetComponent<NameDisplay>();
      this.animator = this.GetComponent<Animator>();
      this.HealthComponent.MaxHealth = 10f;
      this.HealthComponent.CurrentHealth = this.HealthComponent.MaxHealth;
      this.SetupElement();
      this.SetupCombat();
    }

    protected virtual void SetupElement()
    {
      this.SelectedElement = (Element) Game1.Random.Next(0, 5);
      SpriteRenderer spriteRenderer = this.spriteRenderer;
      Color color;
      switch (this.SelectedElement)
      {
        case Element.Fire:
          color = new Color((int) byte.MaxValue, 126, 20);
          break;
        case Element.Water:
          color = new Color(73, 147, 192);
          break;
        case Element.Ice:
          color = new Color(192, 192, 192);
          break;
        case Element.Air:
          color = new Color(6, 162, 101);
          break;
        case Element.Earth:
          color = new Color(50, 0, 74);
          break;
        case Element.Stone:
          color = new Color(177, 143, 115);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      spriteRenderer.Color = color;
    }

    private void SetupCombat()
    {
      this.combat.Init(1f, 1f);
      this.combat.SelectedElement = this.SelectedElement;
      (this.combat.Weapon = (Weapon) new Sword()).Owner = this.GameObject;
    }

    public void Update()
    {
      this.healthbar.IsEnabled = this.IsCurrentTileVisible();
      this.nameDisplay.IsEnabled = this.IsCurrentTileVisible();
      GameTime gameTime = Glob.GameTime;
      this.UpdateBehavior(gameTime);
      this.UpdateBehaviorMovement(gameTime);
      this.UpdateAnimationState();
      this.UpdateRotation();
      this.UpdateMaskRotation();
      this.UpdateInteraction();

      if ((double) this.HealthComponent.CurrentHealth != 0.0)
        return;

      this.Death();
    }

    protected virtual void UpdateMaskRotation()
    {
    }

    protected virtual void UpdateInteraction()
    {
    }

    private void UpdateAnimationState()
    {
      if (this.animator == null)
        return;
      if ((double) this.movement.Velocity.Length() > 0.0)
        this.animator.PlayAnimation("Walk");
      else
        this.animator.PlayAnimation("Idle");
    }

    private void UpdateRotation()
    {
      this.Transform.WorldDirection = MathHelper.ToDegrees(
          (float)Math.Atan2(this.movement.Velocity.X, this.movement.Velocity.Y));
    }

    protected void Attack(Vector2 dir)
    {
      this.combat.AimDirection = (float)Math.Atan2(dir.X, dir.Y);
      this.combat.UseWeapon();
    }

    protected static float GetReactionTime(Npc.BehaviorState behaviorState)
    {
      float reactionTime;
      switch (behaviorState)
      {
        case Npc.BehaviorState.Search:
          reactionTime = 0.25f;
          break;
        case Npc.BehaviorState.Attack:
          reactionTime = 0.2f;
          break;
        default:
          reactionTime = 0.3f;
          break;
      }
      return reactionTime;
    }

    protected abstract void UpdateBehavior(GameTime gameTime);

    protected abstract void UpdateBehaviorMovement(GameTime gameTime);

    private void OnPathfindComplete(ReadOnlyCollection<Vector2Int> path)
    {
      if (path == null)
        return;
      this.path = path;
      this.currentWaypointIndex = 0;
    }

    protected float MoveTo(
      GameTime gameTime,
      Vector2 targetPos,
      float pathfindRate,
      float minDist,
      float maxDist,
      bool needsLineOfSightToPlayer,
      float fleeDist = -1f)
    {
      minDist *= 50f;
      maxDist *= 50f;
      float num1 = float.PositiveInfinity;
      Vector2 vector2 = Vector2.Zero;
      float num2 = 0.0f;
      if (gameTime.TotalGameTime.TotalSeconds > this.lastPathfind + (double) pathfindRate && (this.reachedEndOfPath || this.path == null || this.currentWaypointIndex + 1 >= this.path.Count))
      {
        this.lastPathfind = gameTime.TotalGameTime.TotalSeconds;
        if ((double) fleeDist == -1.0)
          this.seeker.StartPath(this.Transform.Position, targetPos, new Seeker.OnPathfindCompleteDelegate(this.OnPathfindComplete));
        else
          this.seeker.StartPath(this.Transform.Position, fleeDist, new Seeker.OnPathfindCompleteDelegate(this.OnPathfindComplete));
        this.currentWaypointIndex = 0;
      }
      if (this.path != null && this.path.Count > 0)
      {
        this.reachedEndOfPath = false;
        float num3 = Vector2.Distance(this.Transform.Position, targetPos);
        if ((double) num3 < (double) minDist && (!needsLineOfSightToPlayer || this.canSeePlayer))
          this.seeker.StartPath(this.Transform.Position, minDist + 0.75f, new Seeker.OnPathfindCompleteDelegate(this.OnPathfindComplete));
        else if ((double) num3 < (double) maxDist && (!needsLineOfSightToPlayer || this.canSeePlayer))
        {
          this.reachedEndOfPath = true;
          num1 = num3;
          this.lastPathfind = gameTime.TotalGameTime.TotalSeconds;
        }
        else
        {
          float num4;
          while (true)
          {
            num4 = Vector2.Distance(this.Transform.Position, (Vector2) this.path[this.currentWaypointIndex]);
            if ((double) num4 <= 5.0)
            {
              if (this.currentWaypointIndex + 1 < this.path.Count)
                ++this.currentWaypointIndex;
              else
                break;
            }
            else
              goto label_14;
          }
          this.reachedEndOfPath = true;
          num1 = num4;
        }
label_14:
        if (!this.reachedEndOfPath && (!needsLineOfSightToPlayer || this.canSeePlayer))
          num1 = num3;
        if (!this.reachedEndOfPath && this.currentWaypointIndex + 1 < this.path.Count)
        {
          vector2 = ((Vector2) this.path[this.currentWaypointIndex] - this.Transform.Position).Normalized();
          num2 = MathHelper.Clamp(((Vector2) this.path[this.currentWaypointIndex] - this.Transform.Position).LengthSquared() * 4f, 0.75f, 1f);
        }
      }
      this.movement.Move(vector2 * num2);
      return num1;
    }

    protected void ReactToEnemy(GameTime gameTime)
    {
      if (gameTime.TotalGameTime.TotalSeconds < this.nextBravenessCheck)
        return;
      this.CurrentBehaviorState = (double) Game1.Random.NextFloat(0.0f, 1f) 
                < (double) Math.Pow(this.braveness, 0.8f) ? Npc.BehaviorState.Attack : Npc.BehaviorState.Retreat;
      this.nextBravenessCheck = gameTime.TotalGameTime.TotalSeconds + 1.0;
    }

    protected bool IsCurrentTileVisible()
    {
      Tilemap.Tile tile = Tilemap.Instance.GetTile(this.Transform.Position);
      return tile == null || tile.VisibleBy.Count > 0;
    }

    protected static Vector2 GetPosInCircle(float radius)
    {
      float x = Game1.Random.NextFloat(0.0f, 6.28318548f);
      return new Vector2((float)Math.Cos(x), (float)Math.Sin(x)) * radius;
    }

    protected enum BehaviorState
    {
      Idle,
      Search,
      Attack,
      Retreat,
    }
  }
}
