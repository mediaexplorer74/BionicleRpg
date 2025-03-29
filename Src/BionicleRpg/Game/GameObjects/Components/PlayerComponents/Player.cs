
// Type: GameManager.GameObjects.Components.PlayerComponents.Player
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Database;
using GameManager.DataTypes;
using GameManager.Factories;
using GameManager.GameObjects.Components.Items.Masks;
using GameManager.GameObjects.Components.Items.Weapons;
using GameManager.GameObjects.Components.Lighting;
using GameManager.GameObjects.Components.NpcComponents;
using GameManager.GameObjects.Components.Renderers;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.GameObjects.UI;
using GameManager.Map;
using GameManager.Quests;
using GameManager.States;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

#nullable disable
namespace GameManager.GameObjects.Components.PlayerComponents
{
  public class Player : Component//, IElement
  {
    private SpriteRenderer toaSprite;
    private SpriteRenderer eyeSprite;
    private SpriteRenderer maskSprite;
    public SpriteRotation spriteRotation;
    public Element SelectedElement;
    public Color ElementColor;
    private ElementalAbility elementalAbility;
    public Mask MaskComponent;
    private Audio audioComponent;
    private MaskEnergyBar maskEnergyBar;

    public Health HealthComponent { get; set; }

    public Combat CombatComponent { get; set; }

    public Inventory InventoryComponent { get; set; }

    public static Player Instance { get; set; }

    public PlayerController Controller { get; }

    public Movement Movement { get; }

    public float MapZoom { get; set; } = 6f;

    public bool ShowMap { get; set; }

    public Matoran QuestGiver { get; set; }

    public LightEmitter Light { get; }

    public Player() 
    {
        
    }

    public Player(GameObject gameObject) : this()
    {
        this.toaSprite = gameObject.AddComponent<SpriteRenderer>();
        this.eyeSprite = gameObject.AddComponent<SpriteRenderer>();
        this.maskSprite = gameObject.AddComponent<SpriteRenderer>();
        this.SetToaLayerOffset(0.0f);
        this.Light = gameObject.AddComponent<LightEmitter>();
        this.Light.Range = 3;
        this.Light.Intensity = 0.25f;
        Player.Instance = this;
        this.Controller = gameObject.AddComponent<PlayerController>();
        this.Movement = gameObject.AddComponent<Movement>();
        this.Movement.Speed = 200f;
        this.HealthComponent = gameObject.AddComponent<Health>();
        this.HealthComponent.MaxHealth = 100f;
        this.HealthComponent.CurrentHealth = this.HealthComponent.MaxHealth;
        this.CombatComponent = gameObject.AddComponent<Combat>();
        this.InventoryComponent = gameObject.AddComponent<Inventory>();
    }

    public void SetCharacter(Element element)
    {
      this.SetElement(element);
      this.SetMask((MaskType) element);
      this.audioComponent = this.GetComponent<Audio>();
      this.audioComponent?.Play("Mask Change");
    }

    public void SetToaOffset(Vector2 offset)
    {
      this.toaSprite.PosOffset = offset;
      this.eyeSprite.PosOffset = offset;
      this.maskSprite.PosOffset = offset;
    }

    public Vector2 GetToaOffset() => this.toaSprite.PosOffset;

    public void SetToaLayerOffset(float offset)
    {
      this.toaSprite.LayerPosOffset = 0.0f + offset;
      this.eyeSprite.LayerPosOffset = 0.01f + offset;
      this.maskSprite.LayerPosOffset = 0.02f + offset;
    }

    public void Start()
    {
      SpriteRenderer spriteRenderer = this.AddComponent<SpriteRenderer>();
      spriteRenderer.Sprite = Glob.Content.Load<Texture2D>("ToaFloorShadow");
      spriteRenderer.SpriteBatchOverride = Game1.FloorSpriteBatch;
      spriteRenderer.LayerPosOffset = 100f;
      this.spriteRotation = this.GetComponent<SpriteRotation>();
      this.CombatComponent = this.GetComponent<Combat>();
      this.SetCharacter(Element.Fire);
      this.SetupCombatStats();
      this.SetupWeapon();
      DatabaseManager.Instance.Repo.AddCharacter("Test", this.SelectedElement, 
          this.CombatComponent.Weapon.AttackType, (MaskType) this.SelectedElement);
    }

    //RnD : What is it? Why?
    /*private void SetupElement()
    {
      if (this.spriteRotation == null)
        this.spriteRotation = this.GetComponent<SpriteRotation>();
      this.SetElement(Element.Earth);
      this.elementalAbility = this.GameObject.AddComponent<ElementalAbility>();
      this.elementalAbility.Init();
    }*/

    private void SetMask(MaskType type)
    {
      this.MaskComponent?.UnequipMask();
      this.MaskComponent = MaskFactory.Instance.Create(type).GetComponent<Mask>();
      this.MaskComponent.EquipMask();
      UIManager.Instance.UpdateSprite(IconType.Mask, this.MaskComponent.MaskUiSprite);
      this.MaskComponent.Owner = this.GameObject;
      this.maskSprite.Color = this.ElementColor;
      if (this.maskEnergyBar == null)
        this.maskEnergyBar = this.AddComponent<MaskEnergyBar>();
      this.maskEnergyBar.Awake();
      DatabaseManager.Instance.Repo.UpdateCharacterMask(this.SelectedElement, type);
    }

    private void SetupCombatStats()
    {
      this.CombatComponent.Init(1f, 1f);
      this.CombatComponent.SelectedElement = this.SelectedElement;
    }

    private void SetupWeapon()
    {
      this.CombatComponent.Weapon = (Weapon) new Hammer();
      this.CombatComponent.Weapon.Owner = this.GameObject;
      this.CombatComponent.Weapon.UpdateStats();
      DatabaseManager.Instance.Repo.UpdateCharacterWeapon(this.SelectedElement, 
          this.CombatComponent.Weapon.AttackType);
    }

    public void UpdateCombatStats() => this.CombatComponent.Weapon.UpdateStats();

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!this.ShowMap)
        return;
      Vector2 position1 = new Vector2(Game1.ScreenSize.X / 2f, Game1.ScreenSize.Y / 2f);
      Vector2 position2 = position1 - this.Transform.Position * this.MapZoom / 50f;
      Vector2 origin1 = new Vector2((float) UIWorldMap.Instance.Texture.Width / 2f, (float) UIWorldMap.Instance.Texture.Height / 2f);
      Game1.MapSpriteBatch.Draw(UIWorldMap.Instance.Texture, position2, new Rectangle?(), Color.White, 0.0f, origin1, this.MapZoom, SpriteEffects.None, 0.0f);
      Texture2D texture1 = Glob.Content.Load<Texture2D>(this.MaskComponent.MaskUiSprite);
      Vector2 origin2 = new Vector2((float) texture1.Width / 2f, (float) texture1.Height / 2f);
      Game1.MapSpriteBatch.Draw(texture1, position1, new Rectangle?(), this.maskSprite.Color, 0.0f, origin2, 0.4f, SpriteEffects.None, 0.0f);
      Texture2D texture2 = Glob.Content.Load<Texture2D>("QuestMarker");
      Vector2 origin3 = new Vector2((float) texture2.Width / 2f, (float) texture2.Height / 2f);
      for (int index1 = 0; index1 < Quest.Quests.Count; ++index1)
      {
        Quest quest = Quest.Quests[index1];
        if (quest.Complete)
          Game1.MapSpriteBatch.Draw(texture2, position2 + quest.QuestGiver.Transform.Position * this.MapZoom / 50f, new Rectangle?(), Color.White, 0.0f, origin3, 4f, SpriteEffects.None, 0.0f);
        else if (quest is ClearDungeonQuest clearDungeonQuest)
        {
          for (int index2 = 0; index2 < Tilemap.Instance.DungeonEntrances.Count; ++index2)
          {
            DungeonEntrance dungeonEntrance = Tilemap.Instance.DungeonEntrances[index2];
            if (dungeonEntrance.Seed == clearDungeonQuest.DungeonSeed)
              Game1.MapSpriteBatch.Draw(texture2, position2 + dungeonEntrance.Transform.Position * this.MapZoom / 50f, new Rectangle?(), Color.White, 0.0f, origin3, 4f, SpriteEffects.None, 0.0f);
          }
        }
      }
    }

        // Replace the lines with DefaultInterpolatedStringHandler with string interpolation
        public void Update()
        {
            this.RotateMaskSprite();
            this.GetComponent<Animator>().PlayAnimation((double)this.Movement.Velocity.Length() > 0.0 ? "Run" : "Idle");
            this.eyeSprite.SetSprite($"Eyes0{this.spriteRotation.RotationId}");
            Quest.PerformCompletionChecks();
        }

    public void LastUpdate()
    {
      for (int index = 0; index < Matoran.MatoranList.Count; ++index)
      {
        Matoran matoran = Matoran.MatoranList[index];
        matoran.GameObject.SetActive((double) Vector2.Distance(this.Transform.Position, matoran.Transform.Position) < 6000.0);
      }
      if (!(StateManager.Instance.CurrentState is OverworldState))
        return;
      for (int index = 0; index < Enemy.Enemies.Count; ++index)
      {
        Enemy enemy = Enemy.Enemies[index];
        if ((double) Vector2.Distance(this.Transform.Position, enemy.Transform.Position) > 5000.0)
          enemy.GameObject.Destroy();
      }
    }

    public void SetEyeColor(Color color)
    {
      this.eyeSprite.Color = color;
      LightEmitter light = this.Light;
      Color color1 = this.eyeSprite.Color;
      double r = (double) color1.R / (double) byte.MaxValue;
      color1 = this.eyeSprite.Color;
      double g = (double) color1.G / (double) byte.MaxValue;
      color1 = this.eyeSprite.Color;
      double b = (double) color1.B / (double) byte.MaxValue;
      LightColor? color2 = new LightColor?(new LightColor((float) r, (float) g, (float) b));
      int? range = new int?();
      float? intensity = new float?();
      float? halfArc = new float?();
      light.UpdateLightSettings(color2, range, intensity, halfArc);
    }

    public Color GetEyeColor() => this.eyeSprite.Color;

    public void SetElement(Element element)
    {
      switch (element)
      {
        case Element.Fire:
          this.ElementColor = Color.Red;
          this.SetEyeColor(Color.Gold);
          break;
        case Element.Water:
          this.ElementColor = Color.Blue;
          this.SetEyeColor(Color.Yellow);
          break;
        case Element.Ice:
          this.ElementColor = Color.White;
          this.SetEyeColor(Color.Blue);
          break;
        case Element.Air:
          this.ElementColor = Color.Green;
          this.SetEyeColor(Color.GreenYellow);
          break;
        case Element.Earth:
          this.ElementColor = new Color(55, 55, 60);
          this.SetEyeColor(Color.LightGreen);
          break;
        case Element.Stone:
          this.ElementColor = Color.SaddleBrown;
          this.SetEyeColor(Color.OrangeRed);
          break;
      }
      this.toaSprite.Color = this.ElementColor;
      this.SelectedElement = element;
      this.CombatComponent.SelectedElement = element;
      if (this.elementalAbility == null)
      {
        this.elementalAbility = this.AddComponent<ElementalAbility>();
        this.elementalAbility.Init();
      }
      else
        this.elementalAbility.Init();
    }

        private void RotateMaskSprite()
        {
            if (this.maskSprite == null)
                return;
            this.maskSprite.SetSprite($"{this.MaskComponent.MaskSprite}0{this.spriteRotation.RotationId}");
            this.maskSprite.OrderInLayer = -1;
        }
  }
}
