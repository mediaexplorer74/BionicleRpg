
// Type: GameManager.GameObjects.Components.PlayerComponents.PlayerStats
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager.GameObjects.Components.PlayerComponents
{
  public class PlayerStats : Component
  {
    private Health health;
    private Combat elemental;
    private Texture2D statsFrame;
    private Texture2D healthbar;
    private Texture2D elementalbar;
    private Rectangle healthRct;
    private Rectangle elementalRct;

    public PlayerStats()
    { }

    public PlayerStats(GameObject gameObject) : this()
    { }

    public void Awake()
    {
      this.statsFrame = Glob.Content.Load<Texture2D>("UI_StatsFrame");
      this.healthbar = Glob.Content.Load<Texture2D>("UI_PlayerHealth");
      this.elementalbar = Glob.Content.Load<Texture2D>("UI_PlayerElemental");
      this.healthRct = new Rectangle(0, 0, this.healthbar.Width, this.healthbar.Height);
      this.elementalRct = new Rectangle(0, 0, this.elementalbar.Width, this.elementalbar.Height);
    }

    public void Start()
    {
      this.health = Player.Instance.HealthComponent;
      this.elemental = Player.Instance.GetComponent<Combat>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Game1.UISpriteBatch.Draw(this.statsFrame, new Vector2(25f, 25f), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.0f);
      Game1.UISpriteBatch.Draw(this.healthbar, new Vector2((float) sbyte.MaxValue, 60f), new Rectangle?(this.healthRct), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
      Game1.UISpriteBatch.Draw(this.elementalbar, new Vector2(126f, 82.5f), new Rectangle?(this.elementalRct), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
    }

    public void Update()
    {
      this.healthRct.Width = (int) ((double) this.healthbar.Width / (double) this.health.MaxHealth * (double) this.health.CurrentHealth);
      this.elementalRct.Width = (int) ((double) this.elementalbar.Width / (double) this.elemental.MaxElementalEnergy * (double) this.elemental.ElementalEnergy);
    }
  }
}
