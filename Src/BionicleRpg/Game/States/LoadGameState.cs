
// Type: GameManager.States.LoadGameState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Database;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;


namespace GameManager.States
{
  public class LoadGameState : IState
  {
    private List<SavegameData> saveGames;
    private Button[] saveGameButtons;
    private MouseState mousePos;
    private TouchCollection curTouch;
    private int currentButton;
    private int startIndex;

    public Vector2 PreviousButtonPos { get; set; } = 
            new Vector2(
                /*395f*/800f / Game1.screenScale.X ,
                /*380f*/380f / Game1.screenScale.Y);

    public void Enter()
    {
      this.saveGames = DatabaseManager.Instance.Repo.GetAllSavegames();
      this.saveGameButtons = new Button[this.saveGames.Count];

      this.CreateSavegameButtons();

      UIManager.Instance.ShowUIComponent(UIStateAssign.LoadGame, true);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void Update()
    {
    }

    public void Exit()
    {
        UIManager.Instance.ShowUIComponent(UIStateAssign.LoadGame, false);
    }

    public void CreateSavegameButtons()
    {
  

     // use only 3 last savegames
     if (this.saveGames.Count > 3)
       this.startIndex = this.saveGames.Count - 3;

      for (int index = this.startIndex; index < this.saveGames.Count; ++index)
      {
        if (this.saveGameButtons[index] == null)
        {
          Button button = new 
                        Button("UI_ButtonTexture", 
                        this.CalculateButtonPlacement(index));

          button.Click += new EventHandler(this.OnButtonClick);
          button.UIStateAssign = UIStateAssign.LoadGame;
          button.Awake();
          button.Start();

          new Text(this.saveGames[index].ID.ToString(), 
              new Vector2
              (this.PreviousButtonPos.X + 0f, 
              this.PreviousButtonPos.Y + 0f),
              Color.White, 
              1f, 
              TextAlignment.Center).UIStateAssign = UIStateAssign.LoadGame;

          this.saveGameButtons[index] = button;
        }
      }
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
      this.mousePos = Mouse.GetState();
      
      int SaveToLoadID = this.DetermineSaveToLoad();
      DatabaseManager.Instance.LoadSavegame(SaveToLoadID);
      StateManager.Instance.ChangeScreen(
          (IState) new LoadingState((IState) new OverworldState()));
    }

    private Vector2 CalculateButtonPlacement(int currentInt)
    {
        //RnD : button placement    
        Vector2 previousButtonPos = new Vector2
            (450 / Game1.screenScale.X + 800 / Game1.screenScale.X * (currentInt - this.startIndex), 
            this.PreviousButtonPos.Y);

    
      this.PreviousButtonPos = previousButtonPos;
      return previousButtonPos;
    }

    public int DetermineSaveToLoad()
    {
      float touchPosX = 0;
      float touchPosY = 0;

      curTouch = TouchPanel.GetState();
      if (curTouch.Count > 0)
      {
        touchPosX = curTouch[0].Position.X;
        touchPosY = curTouch[0].Position.Y;
      }

      for (int index = this.startIndex; index < this.saveGameButtons.Length; ++index)
      {
        if 
          (this.saveGameButtons[index] != null 
           && (        
              new Rectangle
               ( (int)(this.mousePos.X  / Game1.screenScale.X), 
                 (int)(this.mousePos.Y  / Game1.screenScale.Y), 
                 1, 1)
                .Intersects(this.saveGameButtons[index].Rectangle)   
              )
              ||
               new Rectangle
               ((int)(touchPosX / Game1.screenScale.X),
                 (int)(touchPosY / Game1.screenScale.Y),
                 1, 1)
                .Intersects(this.saveGameButtons[index].Rectangle)
           )
          this.currentButton = index + 1;
      }

      // plan B
      if (this.currentButton < this.startIndex)
            this.currentButton = this.startIndex;
      return this.currentButton;
    }
  }
}
