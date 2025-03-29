
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

#nullable disable
namespace GameManager
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;


    // *********************************************************************
    Vector2 baseScreenSize = new Vector2(1200, 768); //!

    private Matrix globalTransformation;
    int backbufferWidth, backbufferHeight;
    // *********************************************************************


    public static BuilderDirector BuilderDirector = new BuilderDirector();

    
    public static SpriteBatch FloorSpriteBatch { get; private set; }
    public static SpriteBatch FloorDecoSpriteBatch { get; private set; }
    public static SpriteBatch WallSpriteBatch { get; private set; }
    public static SpriteBatch FloorShadowBatch { get; private set; }
    public static SpriteBatch LightingBatch { get; private set; }
    public static SpriteBatch VisionBatch { get; private set; }

    public static Vector2 ScreenSize { get; set; }
    //public float DeltaTime { get; private set; }
    //public GameTime GameTime { get; private set; }

    public static SpriteBatch UISpriteBatch { get; private set; }
    public static SpriteBatch MapSpriteBatch { get; private set; }
    public static Random Random { get; } = new Random();

    public static int screenWidth = 1200;//1920;
    public static int screenHeight = 768;//1080;

    //Game1
    public Game1()
    {
       //RnD
       this.graphics = new GraphicsDeviceManager((Game) this);



#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif


      this.graphics.PreferredBackBufferHeight = 1200;//800;
      this.graphics.PreferredBackBufferWidth = 768;//640;

      Game1.ScreenSize =
                  new Vector2( (float) this.graphics.PreferredBackBufferWidth, 
                              (float) this.graphics.PreferredBackBufferHeight );
        
        //graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft
        //            | DisplayOrientation.LandscapeRight;// | DisplayOrientation.Portrait;
        
        Content.RootDirectory = "Content";
        Glob.Content = Content;

        this.IsMouseVisible = true;
        this.graphics.IsFullScreen = false;//true;

    }//Game1


    // Initialize
    protected override void Initialize()
    {
        //Instance = this;
        
        Glob.GraphicsDevice = GraphicsDevice;

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        DatabaseManager.Instance.CreateRepository();
        
        Quest.Initialize();

        //this.graphics.GraphicsProfile = GraphicsProfile.HiDef; // RnD

        this.graphics.PreferredBackBufferWidth = Game1.screenWidth;
        this.graphics.PreferredBackBufferHeight = Game1.screenHeight;

        this.graphics.ApplyChanges();

        base.Initialize();

        
     }//Initialize


    // ScalePresentationArea - scale our graphics :)
    public void ScalePresentationArea()
    {
        //Work out how much we need to scale our graphics to fill the screen
        backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth - 0; // 40
        backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        float horScaling = backbufferWidth / baseScreenSize.X;
        float verScaling = backbufferHeight / baseScreenSize.Y;

        Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);

        globalTransformation = Matrix.CreateScale(screenScalingFactor);

        System.Diagnostics.Debug.WriteLine("Screen Size - Width["
            + GraphicsDevice.PresentationParameters.BackBufferWidth + "] " +
            "Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");
    }


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
      //      ScalePresentationArea();
      // ************************************** 

      StateManager.Instance.AddScreen((IState) new MenuState());
   }//LoadContent

    public static void CreateWorld(int width, int height)
    {
      Game1.CreateTilemaps(width, height);
      Game1.BuilderDirector.ConstructPlayer();

      if (DatabaseManager.Instance.IsNewGame)
        DatabaseManager.Instance.NewSavegame();
      else
        DatabaseManager.Instance.SetLoadVariables();
    }

    public static void InitGameObjects()
    {
      List<GameObject> list = GameObject.GameObjects.ToList<GameObject>();
      foreach (GameObject gameObject in list)
        gameObject.Awake();
      foreach (GameObject gameObject in list)
        gameObject.Start();
    }

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
    }

    protected override void Update(GameTime gameTime)
    {
      // *********************************
      //Confirm the screen has not been resized by the user
      //if (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight ||
      //  backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
      //{
      //  ScalePresentationArea();
      //}
      // *********************************


      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
        || Keyboard.GetState().IsKeyDown(Keys.Escape))
        this.Exit();

      //Glob.DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
      //Glob.GameTime = gameTime;

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
    }

    protected /*override*/ void Draw1(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(/*Color.Black*/Color.AliceBlue);

      //Game1.FloorSpriteBatch.Begin(SpriteSortMode.BackToFront,   BlendState.AlphaBlend, SamplerState.PointClamp, 
      //    null, null,null, globalTransformation);

      //  Game1.FloorDecoSpriteBatch.Begin(SpriteSortMode.BackToFront,   BlendState.AlphaBlend, SamplerState.PointClamp,
      //  null, null, null, globalTransformation);

      //  Game1.FloorShadowBatch.Begin(SpriteSortMode.BackToFront,  BlendState.AlphaBlend, SamplerState.PointClamp,
      //  null, null, null, globalTransformation);

      //  Game1.WallSpriteBatch.Begin(SpriteSortMode.BackToFront,     BlendState.AlphaBlend, SamplerState.PointClamp,
      //  null, null, null, globalTransformation);

      //  Game1.VisionBatch.Begin(SpriteSortMode.BackToFront,  BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
      //    null, null, null, globalTransformation);

      //  Game1.UISpriteBatch.Begin(SpriteSortMode.BackToFront,  BlendState.AlphaBlend, SamplerState.LinearClamp,
      //    null, null, null, globalTransformation);

      //  Game1.MapSpriteBatch.Begin(SpriteSortMode.BackToFront,  BlendState.AlphaBlend, SamplerState.PointClamp,
      //    null, null, null, globalTransformation);

      /*  Game1.LightingBatch.Begin(blendState: new BlendState()
        {
        ColorBlendFunction = BlendFunction.Add,
        ColorSourceBlend = Blend.DestinationColor,
        ColorDestinationBlend = Blend.Zero
        }, samplerState: SamplerState.AnisotropicClamp);*/

      //  for (int index = GameObject.GameObjects.Count - 1; index >= 0; --index)
      //  GameObject.GameObjects[index].Draw(Game1.WallSpriteBatch);

        StateManager.Instance.Draw(Game1.WallSpriteBatch);
        UIManager.Instance.Draw(Game1.UISpriteBatch);

      //  Game1.FloorSpriteBatch.End();
      //  Game1.FloorDecoSpriteBatch.End();
      //  Game1.FloorShadowBatch.End();
      //  Game1.WallSpriteBatch.End();
      //  Game1.LightingBatch.End();
      //  Game1.VisionBatch.End();
      //  Game1.UISpriteBatch.End();
      //  Game1.MapSpriteBatch.End();            

        base.Draw(gameTime);
    }
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            Game1.FloorSpriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            Game1.FloorDecoSpriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            Game1.FloorShadowBatch.Begin(samplerState: SamplerState.AnisotropicClamp);

            Game1.WallSpriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            Game1.VisionBatch.Begin(samplerState: SamplerState.AnisotropicClamp);

            Game1.UISpriteBatch.Begin(samplerState: SamplerState.LinearClamp);

            Game1.MapSpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Game1.LightingBatch.Begin(blendState: new BlendState()
            {
                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.DestinationColor,
                ColorDestinationBlend = Blend.Zero
            }, samplerState: SamplerState.AnisotropicClamp);

            for (int index = GameObject.GameObjects.Count - 1; index >= 0; --index)
            {
                GameObject.GameObjects[index].Draw(Game1.WallSpriteBatch);
            }


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
        }

    }
}
