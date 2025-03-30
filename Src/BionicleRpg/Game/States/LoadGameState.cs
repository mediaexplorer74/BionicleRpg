
// Type: GameManager.States.LoadGameState
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Database;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace GameManager.States
{
  public class LoadGameState : IState
  {
    private List<SavegameData> saveGames;
    private Button[] saveGameButtons;
    private MouseState mousePos;
    private int currentButton;

    public Vector2 PreviousButtonPos { get; set; } = new Vector2(365f, 380f);

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

    public void Exit() => UIManager.Instance.ShowUIComponent(UIStateAssign.LoadGame, false);

    public void CreateSavegameButtons()
    {
      for (int index = 0; index < this.saveGames.Count; ++index)
      {
        if (this.saveGameButtons[index] == null)
        {
          Button button = new Button("UI_ButtonTexture", this.CalculateButtonPlacement(index));
          button.Click += new EventHandler(this.OnButtonClick);
          button.UIStateAssign = UIStateAssign.LoadGame;
          button.Awake();
          button.Start();

          new Text(this.saveGames[index].ID.ToString(), 
              new Vector2(this.PreviousButtonPos.X, this.PreviousButtonPos.Y - 25f), 
              Color.White, 2f, TextAlignment.Center).UIStateAssign = UIStateAssign.LoadGame;

          this.saveGameButtons[index] = button;
        }
      }
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
      this.mousePos = Mouse.GetState();
      DatabaseManager.Instance.LoadSavegame(this.DetermineSaveToLoad());
      StateManager.Instance.ChangeScreen((IState) new LoadingState((IState) new OverworldState()));
    }

    private Vector2 CalculateButtonPlacement(int currentInt)
    {
      Vector2 previousButtonPos = this.PreviousButtonPos;
      switch (currentInt)
      {
        case 0:
          return this.PreviousButtonPos;
        case 4:
          if (currentInt == 4)
          {
            previousButtonPos.X = 365f;
            previousButtonPos.Y += 300f;
            break;
          }
          break;
        default:
          if (this.saveGames.Count != 0)
          {
            previousButtonPos.X += 400f;
            break;
          }
          goto case 4;
      }
      this.PreviousButtonPos = previousButtonPos;
      return previousButtonPos;
    }

    public int DetermineSaveToLoad()
    {
      for (int index = 0; index < this.saveGameButtons.Length; ++index)
      {
        if (this.saveGameButtons[index] != null 
                    && new Rectangle(this.mousePos.X, this.mousePos.Y, 1, 1)
                    .Intersects(this.saveGameButtons[index].Rectangle))
          this.currentButton = index + 1;
      }
      return this.currentButton;
    }
  }
}
