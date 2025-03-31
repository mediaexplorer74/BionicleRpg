
// Type: GameManager.GameWorld
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.Builders;
using GameManager.Database;
using GameManager.GameObjects;
using GameManager.GameObjects.Components.Renderers;
using GameManager.GameObjects.Components.Tilemaps;
using GameManager.Quests;
using GameManager.States;
using GameManager.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace GameManager
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;

    // *********************************************************************
    Vector2 baseScreenSize = new Vector2(1024, 1080 / 2); 
    private Matrix globalTransformation;
    int backbufferWidth, backbufferHeight;
    public static bool FirstResize = true;
    public static Vector3 screenScale;
    // *********************************************************************


    public static BuilderDirector BuilderDirector = new BuilderDirector();

    public static Game1 Instance { get; private set; }
    public static SpriteBatch FloorSpriteBatch { get; private set; }
    public static SpriteBatch FloorDecoSpriteBatch { get; private set; }
    public static SpriteBatch WallSpriteBatch { get; private set; }
    public static SpriteBatch FloorShadowBatch { get; private set; }
    public static SpriteBatch LightingBatch { get; private set; }
    public static SpriteBatch VisionBatch { get; private set; }

    public static int screenWidth = 1024;//1920;
    public static int screenHeight = 1080 / 2;//1080;
    public static Vector2 ScreenSize { get; set; }

    public static SpriteBatch UISpriteBatch { get; private set; }
    public static SpriteBatch MapSpriteBatch { get; private set; }
    public static Random Random { get; } = new Random();

    public const int WorldSizeX = 500;//1000;
    public const int WorldSizeY = 500;//1000;


    // Game1
    public Game1()
    {
    this.graphics = new GraphicsDeviceManager((Game) this);
#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif
        Game1.ScreenSize = new Vector2(screenWidth, screenHeight);
        graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft
          | DisplayOrientation.LandscapeRight | DisplayOrientation.Portrait;        
        Content.RootDirectory = "Content";
        Glob.Content = Content;
        this.IsMouseVisible = true;  
    }//Game1


    // Initialize
    protected override void Initialize()
    {
        Instance = this;        
        Glob.GraphicsDevice = GraphicsDevice;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        DatabaseManager.Instance.CreateRepository();        
        Quest.Initialize();
        //this.graphics.GraphicsProfile = GraphicsProfile.Reach; // RnD
        //Game1.ScreenSize = new Vector2(screenWidth, screenHeight);
        this.graphics.PreferredBackBufferWidth = Game1.screenWidth;
        this.graphics.PreferredBackBufferHeight = Game1.screenHeight;
        this.graphics.IsFullScreen = true; // set *false* only for better debug
        this.graphics.ApplyChanges();
        base.Initialize();        
     }//Initialize


    // ScalePresentationArea
    public void ScalePresentationArea()
    {
        //Work out how much we need to scale our graphics to fill the screen
        backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth - 0; // 40
        backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        float horScaling = (float)(backbufferWidth * 1f / baseScreenSize.X);
        float verScaling = (float)(backbufferHeight *1f / baseScreenSize.Y);

        screenScale = new Vector3(horScaling, verScaling, 1);

        globalTransformation = Matrix.CreateScale(screenScale);

        //System.Diagnostics.Debug.WriteLine("Screen Size - Width["
        //    + GraphicsDevice.PresentationParameters.BackBufferWidth + "] " +
        //    "Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");

    }//ScalePresentationArea


    // LoadContent
    protected override void LoadContent()
    {
      
      UIManager.Instance.LoadContent();

      Game1.FloorSpriteBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.FloorDecoSpriteBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.FloorShadowBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.WallSpriteBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.LightingBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.VisionBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.UISpriteBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.MapSpriteBatch = new SpriteBatch(this.GraphicsDevice);

      // **************************************
      ScalePresentationArea();
      // ************************************** 

      StateManager.Instance.AddScreen((IState) new MenuState());
    }//LoadContent


    // CreateWorld
    public static void CreateWorld(int width, int height)
    {
      Game1.CreateTilemaps(width, height);
      Game1.BuilderDirector.ConstructPlayer();

      if (DatabaseManager.Instance.IsNewGame)
        DatabaseManager.Instance.NewSavegame();
      else
        DatabaseManager.Instance.SetLoadVariables();
    }//CreateWorld

    // InitGameObjects
        public static void InitGameObjects()
    {
      List<GameObject> list = GameObject.GameObjects.ToList<GameObject>();
      foreach (GameObject gameObject in list)
        gameObject.Awake();
      foreach (GameObject gameObject in list)
        gameObject.Start();
    }//InitGameObjects


    // CreateTilemaps
    private static void CreateTilemaps(int width, int height)
    {
      Tilemap tilemap = new GameObject().AddComponent<Tilemap>();
      tilemap.Width = width;
      tilemap.Height = height;

      Vector2 vector2 = new Vector2((float) -Tilemap.Instance.HalfWidth - 0.5f,
          (float) -Tilemap.Instance.HalfHeight - 0.5f) * 50f;

      GameObject gameObject1 = new GameObject();
      gameObject1.Transform.Position = vector2;
      gameObject1.Transform.Scale = 50f;
      GenericRenderer genericRenderer1 = gameObject1.AddComponent<GenericRenderer>();
      genericRenderer1.Renderable = (IRenderable) gameObject1.AddComponent<EdgeShadowTilemap>();
      genericRenderer1.SpriteBatchOverride = Game1.FloorShadowBatch;
      GameObject gameObject2 = new GameObject();
      gameObject2.Transform.Position = vector2;
      gameObject2.Transform.Scale = 50f;
      GenericRenderer genericRenderer2 = gameObject2.AddComponent<GenericRenderer>();
      genericRenderer2.Renderable = (IRenderable) gameObject2.AddComponent<LightingTilemap>();
      genericRenderer2.SpriteBatchOverride = Game1.LightingBatch;
      GameObject gameObject3 = new GameObject();
      gameObject3.Transform.Position = vector2;
      gameObject3.Transform.Scale = 50f;
      GenericRenderer genericRenderer3 = gameObject3.AddComponent<GenericRenderer>();
      genericRenderer3.Renderable = (IRenderable) gameObject3.AddComponent<VisionTilemap>();
      genericRenderer3.SpriteBatchOverride = Game1.VisionBatch;
    }//CreateTilemaps


    // Update
    protected override void Update(GameTime gameTime)
    {
      // *********************************
      //Check First Resize  & Control time where Screen has not been resized by the user
      if ( FirstResize 
           ||
           (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight 
           ||
           backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
           )
      {
        ScalePresentationArea();
        FirstResize = false;
      }
      // *********************************

      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
        || Keyboard.GetState().IsKeyDown(Keys.Escape))
        this.Exit();


      Glob.DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
      Glob.GameTime = gameTime;

      List<GameObject> list = GameObject.GameObjects.ToList<GameObject>();

      foreach (GameObject gameObject in list)
        gameObject.Update();

      foreach (GameObject gameObject in list)
        gameObject.LateUpdate();

      foreach (GameObject gameObject in list)
        gameObject.LastUpdate();

      StateManager.Instance.Update();
      UIManager.Instance.Update();

      base.Update(gameTime);
    }//Update

    // Draw
    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(Color.Black);

      Game1.FloorSpriteBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, BlendState.AlphaBlend,
          SamplerState.PointClamp, null, null,null, globalTransformation);

      Game1.FloorDecoSpriteBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, BlendState.AlphaBlend,
          SamplerState.PointClamp,  null, null, null, globalTransformation);

      Game1.FloorShadowBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, BlendState.AlphaBlend,
          SamplerState.AnisotropicClamp, null, null, null, globalTransformation);

      Game1.WallSpriteBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, BlendState.AlphaBlend, 
          SamplerState.PointClamp, null, null, null, globalTransformation);

        Game1.VisionBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, BlendState.AlphaBlend, 
            SamplerState.AnisotropicClamp, null, null, null, globalTransformation);

        Game1.UISpriteBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred,  
            BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, globalTransformation);

        Game1.MapSpriteBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, BlendState.AlphaBlend, 
            SamplerState.PointClamp,  null, null, null, globalTransformation);

        Game1.LightingBatch.Begin(/*SpriteSortMode.BackToFront*/SpriteSortMode.Deferred, new BlendState()
           { ColorBlendFunction = BlendFunction.Add, 
            ColorSourceBlend = Blend.DestinationColor, 
            ColorDestinationBlend = Blend.Zero }, 
           SamplerState.AnisotropicClamp, null,null,null, globalTransformation);

        for (int index = GameObject.GameObjects.Count - 1; index >= 0; --index)
          GameObject.GameObjects[index].Draw(Game1.WallSpriteBatch);

        StateManager.Instance.Draw(Game1.WallSpriteBatch);
        UIManager.Instance.Draw(Game1.UISpriteBatch);

        Game1.FloorSpriteBatch.End();
        Game1.FloorDecoSpriteBatch.End();
        Game1.FloorShadowBatch.End();
        Game1.WallSpriteBatch.End();
        Game1.LightingBatch.End();
        Game1.VisionBatch.End();
        Game1.UISpriteBatch.End();
        Game1.MapSpriteBatch.End();            

        base.Draw(gameTime);
    }//Draw
  }
}
