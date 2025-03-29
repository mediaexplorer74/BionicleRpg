
// Type: GameManager.UI.UIManager
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.GameObjects.UI;
using GameManager.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Windows.Media.ContentRestrictions;

#nullable disable
namespace GameManager.UI
{
  public class UIManager
  {
    private Image playerMaskIcon;
    private Image maskIcon;
    private Image powerIcon;
    private Texture2D maskIconSprite;
    private static UIManager instance;

    public string MaskIconName { get; set; } = "Icon_LevitationMask";

    public SpriteFont UIFont{ get; private set; } 

    public static UIManager Instance
    {
      get
      {
        if (UIManager.instance == null)
          UIManager.instance = new UIManager();
        return UIManager.instance;
      }
    }

    public void LoadContent()
    {
      this.UIFont = Glob.Content.Load<SpriteFont>("UIFont");
      this.CreateImages();
      this.CreateButtons();

      for (int index = 0; index < UIComponent.UIComponents.Count; ++index)
        UIComponent.UIComponents[index].Awake();

      for (int index = 0; index < UIComponent.UIComponents.Count; ++index)
        UIComponent.UIComponents[index].Start();
    }

    public void Update()
    {
      for (int index = 0; index < UIComponent.UIComponents.Count; ++index)
      {
        if (UIComponent.UIComponents[index].IsShowing)
          UIComponent.UIComponents[index].Update();
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int index = 0; index < UIComponent.UIComponents.Count; ++index)
      {
        if (UIComponent.UIComponents[index].IsShowing)
          UIComponent.UIComponents[index].Draw(spriteBatch);
      }
    }

    public void CreateImages()
    {
      Image image1 = new Image("UI_Hotbar_LMB", new Vector2(Game1.ScreenSize.X / 2f, 
          Game1.ScreenSize.Y - 75f), 0.6f);

      image1.IconType = IconType.Other;
      image1.UIStateAssign = UIStateAssign.Gameplay;
      this.maskIcon = new Image(this.MaskIconName, 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 268.0),
          Game1.ScreenSize.Y - 70f), 0.35f);

      this.maskIcon.IconType = IconType.Mask;
      this.maskIcon.Color = Color.LightGray;
      this.maskIcon.UIStateAssign = UIStateAssign.Gameplay;
      this.playerMaskIcon = new Image(this.MaskIconName, new Vector2(77f, 74f), 0.55f);
      this.playerMaskIcon.IconType = IconType.Other;
      this.playerMaskIcon.UIStateAssign = UIStateAssign.Gameplay;

      Image image2 = new Image("AirIcon", 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 170.0), 
          Game1.ScreenSize.Y - 70f), 0.3f);
      image2.IconType = IconType.ElementIcon;
      image2.UIStateAssign = UIStateAssign.Gameplay;

      Image image3 = new Image("FireIcon", 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 105.0), 
          Game1.ScreenSize.Y - 70f), 0.3f);
      image3.IconType = IconType.ElementIcon;
      image3.UIStateAssign = UIStateAssign.Gameplay;

      Image image4 = new Image("EarthIcon", 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 - 33.0), 
          Game1.ScreenSize.Y - 70f), 0.3f);
      image4.IconType = IconType.ElementIcon;
      image4.UIStateAssign = UIStateAssign.Gameplay;

      Image image5 = new Image("WaterIcon", 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 35.0), 
          Game1.ScreenSize.Y - 70f), 0.3f);
      image5.IconType = IconType.ElementIcon;
      image5.UIStateAssign = UIStateAssign.Gameplay;

      Image image6 = new Image("StoneIcon", 
          new Vector2(1064.28577f, Game1.ScreenSize.Y - 70f), 0.3f);
      image6.IconType = IconType.ElementIcon;
      image6.UIStateAssign = UIStateAssign.Gameplay;

      Image image7 = new Image("IceIcon", 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 172.0), 
          Game1.ScreenSize.Y - 70f), 0.3f);
      image7.IconType = IconType.ElementIcon;
      image7.UIStateAssign = UIStateAssign.Gameplay;

      this.powerIcon = new Image("AirIcon", 
          new Vector2(1231.42859f, Game1.ScreenSize.Y - 70f), 0.3f);
      this.powerIcon.IconType = IconType.ElementPower;
      this.powerIcon.UIStateAssign = UIStateAssign.Gameplay;


      new Image("UI_MenuScreen", 
          new Vector2(Game1.ScreenSize.X / 2f, 
          Game1.ScreenSize.Y / 2f), 1f).UIStateAssign = UIStateAssign.StartMenu;

      new Image("UI_LoadGameScreen", 
          new Vector2(Game1.ScreenSize.X / 2f,
          Game1.ScreenSize.Y / 2f), 1f).UIStateAssign = UIStateAssign.LoadGame;

      new Image("UI_LoadingScreen", 
          new Vector2(Game1.ScreenSize.X / 2f, 
          Game1.ScreenSize.Y / 2f), 1f).UIStateAssign = UIStateAssign.Loading;


      Image image8 = new Image("UI_QuestDisplay", new Vector2(Game1.ScreenSize.X - 210f, 150f), 0.8f);
      image8.UIStateAssign = UIStateAssign.Gameplay;
      image8.IconType = IconType.Other;
    }

    public void CreateButtons()
    {
      Button button1 = new Button("Button_NewGame", 
          new Vector2(Game1.ScreenSize.X / 2f, Game1.ScreenSize.Y / 2f));
      button1.UIStateAssign = UIStateAssign.StartMenu;

      button1.Click += new EventHandler(this.NewGameButton_Click);

      Button button2 = new Button("Button_LoadGame", 
          new Vector2(Game1.ScreenSize.X / 2f, 
          (float) ((double) Game1.ScreenSize.Y / 2.0 + 75.0)));
      button2.UIStateAssign = UIStateAssign.StartMenu;

      button2.Click += new EventHandler(this.LoadGameButton_Click);

      Button button3 = new Button("Button_QuitGame", 
          new Vector2((float) ((double) Game1.ScreenSize.X / 2.0 + 5.0), 
          (float) ((double) Game1.ScreenSize.Y / 2.0 + 150.0)));
      button3.UIStateAssign = UIStateAssign.StartMenu;

      button3.Click += new EventHandler(this.QuitGameButton_Click);

      Button button4 = new Button("Button_Return", 
          new Vector2(Game1.ScreenSize.X / 2f, Game1.ScreenSize.Y - 150f));
      button4.UIStateAssign = UIStateAssign.LoadGame;
      button4.Click += new EventHandler(this.ReturnButton_Click);
    }

    public void UpdateSprite(IconType iconType, string spriteName)
    {
      if (iconType != IconType.Mask)
      {
        if (iconType != IconType.ElementPower)
          return;
        this.powerIcon.Sprite = Glob.Content.Load<Texture2D>(spriteName);
      }
      else
      {
        this.maskIconSprite = Glob.Content.Load<Texture2D>(spriteName);
        this.playerMaskIcon.Sprite = this.maskIconSprite;
        this.maskIcon.Sprite = this.maskIconSprite;
      }
    }

    public void ShowUIComponent(UIStateAssign currentScreen, bool shouldShow)
    {
      foreach (UIComponent uiComponent in UIComponent.UIComponents)
      {
        if (uiComponent.UIStateAssign == currentScreen)
          uiComponent.IsShowing = shouldShow;
      }
    }

    private void ReturnButton_Click(object sender, EventArgs e)
    {
      StateManager.Instance.RemoveScreen();
    }

    private void QuitGameButton_Click(object sender, EventArgs e)
    {
       
       Game1.Instance.Exit();       
    }

    private void LoadGameButton_Click(object sender, EventArgs e)
    {
      StateManager.Instance.AddScreen((IState) new LoadGameState());
    }

    private void NewGameButton_Click(object sender, EventArgs e)
    {
      StateManager.Instance.ChangeScreen((IState) new LoadingState((IState) new OverworldState()));
    }
  }
}
