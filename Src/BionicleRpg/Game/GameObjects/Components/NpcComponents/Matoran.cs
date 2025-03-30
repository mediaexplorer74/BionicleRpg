
// Type: GameManager.GameObjects.Components.NpcComponents.Matoran
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.PlayerComponents;
using GameManager.GameObjects.Components.Renderers;
using GameManager.Quests;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;


namespace GameManager.GameObjects.Components.NpcComponents
{
  public class Matoran : Npc
  {
    private static readonly List<Matoran> matoran = new List<Matoran>();
    private double retreatUntil = double.PositiveInfinity;
    private Vector2 retreatFrom;
    private string maskName;
    private SpriteRenderer speechBubbleRenderer;
    private string indicatorText = "Press F to talk";
    private float interactionDistance = 50f;
    private Vector2 originPos;
    private Vector2? wanderPos;
    private float nextWanderPosUpdate = float.NegativeInfinity;
    private const float IdleMovementSpeed = 75f;
    private const float RunMovementSpeed = 200f;

    public static ReadOnlyCollection<Matoran> MatoranList => Matoran.matoran.AsReadOnly();

    public SpriteRenderer MaskSprite { get; set; }

    public SpriteRotation SpriteRotation { get; set; }

    public Quest Quest { get; set; }

    private bool CanInteract => Player.Instance.QuestGiver == this;

     
    public Matoran()
    {
        this.Name = "Matoran Villager";
        switch (Game1.Random.Next(0, 5))
        {
            case 0:
                this.maskName = "Huna";
                break;
            case 1:
                this.maskName = "Ruru";
                break;
            case 2:
                this.maskName = "Rau";
                break;
            case 3:
                this.maskName = "Mahiki";
                break;
            case 4:
                this.maskName = "Matatu";
                break;
            case 5:
                this.maskName = "Komau";
                break;
        }
        Matoran.matoran.Add(this);
    }

    public Matoran(GameObject gameObject) : this()
    {
               
    }

    protected override Npc.BehaviorState CurrentBehaviorState
    {
      get => base.CurrentBehaviorState;
      set
      {
        if (value == Npc.BehaviorState.Idle)
        {
          this.movement.Speed = 75f;
          this.animator.FpsMultiplier = 0.375f;
        }
        else
        {
          this.movement.Speed = 200f;
          this.animator.FpsMultiplier = 1f;
        }
        if (base.CurrentBehaviorState != value)
          this.path = (ReadOnlyCollection<Vector2Int>) null;
        base.CurrentBehaviorState = value;
      }
    }

    

    public void Start()
    {
      this.CurrentBehaviorState = Npc.BehaviorState.Idle;
      this.originPos = this.Transform.Position;
      if (Game1.Random.Next(0, 2) != 0)
        return;
      this.Quest = Quest.GenerateRandom(this, this.Transform.Position);
      if (this.Quest == null)
        return;
      this.SetQuestSpeechBubbleEnabled(true);
    }

    public void SetQuestSpeechBubbleEnabled(bool enabled)
    {
      if (enabled)
      {
        this.speechBubbleRenderer = this.AddComponent<SpriteRenderer>();
        this.speechBubbleRenderer.PosOffset = new Vector2(0.0f, -2.5f);
        this.speechBubbleRenderer.ScaleMultiplier = 1.25f;
        this.speechBubbleRenderer.Sprite = Glob.Content.Load<Texture2D>("Matoran_Speechbubble");

        if (!this.Quest.Complete)
          return;
        this.speechBubbleRenderer.Color = Color.LightGreen;
      }
      else
        this.RemoveComponent((Component) this.speechBubbleRenderer);
    }

    public static void ClearAll()
    {
        Matoran.matoran.Clear();
    }

    protected override void SetupElement()
    {
      this.SelectedElement = (Element) Game1.Random.Next(0, 5);
      SpriteRenderer component = this.GetComponent<SpriteRenderer>();
      Color color1;
      switch (this.SelectedElement)
      {
        case Element.Fire:
          color1 = Color.Red;
          break;
        case Element.Water:
          color1 = Color.Blue;
          break;
        case Element.Ice:
          color1 = Color.White;
          break;
        case Element.Air:
          color1 = Color.Green;
          break;
        case Element.Earth:
          color1 = new Color(55, 55, 60);
          break;
        case Element.Stone:
          color1 = Color.SaddleBrown;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      component.Color = color1;
      SpriteRenderer maskSprite = this.MaskSprite;
      Color color2;
      switch (this.SelectedElement)
      {
        case Element.Fire:
          color2 = new Color((int) byte.MaxValue, 126, 20);
          break;
        case Element.Water:
          color2 = new Color(73, 147, 192);
          break;
        case Element.Ice:
          color2 = new Color(192, 192, 192);
          break;
        case Element.Air:
          color2 = new Color(6, 162, 101);
          break;
        case Element.Earth:
          color2 = new Color(50, 0, 74);
          break;
        case Element.Stone:
          color2 = new Color(177, 143, 115);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      maskSprite.Color = color2;
    }

    public void AcceptQuest()
    {
      if (this.Quest.IsAccepted)
        return;
      this.SetQuestSpeechBubbleEnabled(false);
      Quest.AcceptQuest(this.Quest);
      Player.Instance.QuestGiver = (Matoran) null;
    }

    protected override void UpdateInteraction()
    {
      if (this.Quest == null || this.Quest.IsAccepted && !this.Quest.Complete)
        return;
      if (Quest.Quests.Count >= 5)
      {
        if (this.speechBubbleRenderer == null)
          return;
        this.SetQuestSpeechBubbleEnabled(false);
      }
      else
      {
        if (this.speechBubbleRenderer == null && !this.Quest.IsAccepted)
          this.SetQuestSpeechBubbleEnabled(true);

        if ((double) (Player.Instance.Transform.Position - 
                    this.Transform.Position).Length() > (double) this.interactionDistance)
        {
          if (Player.Instance.QuestGiver != this)
            return;
          Player.Instance.QuestGiver = (Matoran) null;
        }
        else
          Player.Instance.QuestGiver = this;
      }
    }

        protected override void UpdateMaskRotation()
        {
            this.MaskSprite.LayerPosOffset = 0.5f;
            SpriteRenderer maskSprite = this.MaskSprite;
            string spriteName = $"{this.maskName}0{this.SpriteRotation.RotationId}";
            maskSprite.SetSprite(spriteName);
        }

    protected override void InitializeBehaviorValues()
    {
      this.braveness = 0.0f;
      base.InitializeBehaviorValues();
    }

    protected override void UpdateBehavior(GameTime gameTime)
    {
      for (int index = 0; index < Enemy.Enemies.Count; ++index)
      {
        Enemy enemy = Enemy.Enemies[index];
        if ((double) Vector2.Distance(this.Transform.Position, enemy.Transform.Position) <= 400.0)
        {
          this.ReactToEnemy(gameTime);
          if (this.CurrentBehaviorState != Npc.BehaviorState.Retreat)
            break;
          this.retreatUntil = gameTime.TotalGameTime.TotalSeconds + 20.0;
          this.retreatFrom = enemy.Transform.Position;
          break;
        }
      }
    }

    protected override void UpdateBehaviorMovement(GameTime gameTime)
    {
      switch (this.CurrentBehaviorState)
      {
        case Npc.BehaviorState.Idle:
          if (!this.wanderPos.HasValue || gameTime.TotalGameTime.TotalSeconds >= (double) this.nextWanderPosUpdate)
          {
            this.wanderPos = new Vector2?(this.originPos + Npc.GetPosInCircle(30f) * 50f);
            this.nextWanderPosUpdate = (float) gameTime.TotalGameTime.TotalSeconds 
                            + Game1.Random.NextFloat(10f, 80f);
            break;
          }
          double num = (double) this.MoveTo(gameTime, this.wanderPos.Value, 25f, 0.0f, 1f, false);
          break;
        case Npc.BehaviorState.Retreat:
          if (gameTime.TotalGameTime.TotalSeconds <= this.retreatUntil && (double) this.MoveTo(gameTime, this.retreatFrom, 4f, 0.0f, 0.1f, false, 50f) > 75.0)
            break;
          this.CurrentBehaviorState = Npc.BehaviorState.Idle;
          break;
        default:
          throw new NotImplementedException();
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!this.CanInteract)
        return;
      Vector2 position = new Vector2((float) ((double) this.Transform.Position.X - (double) Player.Instance.Transform.Position.X + (double) Game1.ScreenSize.X / 2.0 - (double) UIManager.Instance.UIFont.MeasureString(this.indicatorText).X / 2.0 * 0.5), (float) ((double) this.Transform.Position.Y - (double) Player.Instance.Transform.Position.Y + (double) Game1.ScreenSize.Y / 2.0 - 75.0));
      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, this.indicatorText, position + Vector2.One, Color.Black, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
      Game1.UISpriteBatch.DrawString(UIManager.Instance.UIFont, this.indicatorText, position, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
    }
  }
}
