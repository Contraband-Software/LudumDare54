namespace LD54;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Leviathan;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Contracts;

public class App_Julius : Game
{
    private LeviathanEngine engine;
    private Random rnd = new Random();
    private GraphicsDeviceManager graphics;
    // contentManager

    public App_Julius()
    {
        this.Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
        graphics = new GraphicsDeviceManager(this);
        //graphics.PreferredBackBufferWidth = 2560; //FIX THIS
        //graphics.PreferredBackBufferHeight = 1440;

    }

    protected override void Initialize() {
        engine = new LeviathanEngine(this);
        this.Components.Add(engine);
        this.Services.AddService(typeof(ILeviathanEngineService), engine);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                engine.AddLight(new Vector2(i * 80, j * 80), new Vector3(i * 100, j * 100, 50));
            }
        }
        //engine.AddLight(new Vector2(150, 150), new Vector3(1000, 1000, 1000));

        Texture2D colortex = Content.Load<Texture2D>("Sprites/image");
        Texture2D normaltex = Content.Load<Texture2D>("Sprites/normal");

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                LeviathanSprite testSprite = new LeviathanSprite(this, Matrix.CreateTranslation(new Vector3(i * 120, j * 120, 0)), new Point(100), colortex, normaltex);
                engine.addSprite(testSprite);
            }
        }

        SpriteFont testFont = Content.Load<SpriteFont>("Fonts/main");
        engine.addUISprite(new LeviathanUIElement(this, Matrix.CreateTranslation(new Vector3(0)),new Point(100), colortex));
        engine.addUISprite(new LeviathanUIElement(this, Matrix.CreateTranslation(new Vector3(100,400,0)), new Point(10), "hello world", testFont, Color.Red));
        LeviathanShader bloomShader = new LeviathanShader(this, "Shaders/bloom");
        bloomShader.AddParam("strength", 0.5f);
        bloomShader.AddParam("brightnessThreshold", 20f);
        engine.addPostProcess(bloomShader);

        LeviathanShader abberationShader = new LeviathanShader(this,"Shaders/abberation");
        abberationShader.AddParam("strength", 0.004f);
        engine.addPostProcess(abberationShader);

        base.Initialize();

        PrintLn("Game initialized");
    }
     
    protected override void LoadContent()
    {
        // TODO: GLOBAL LOAD CONTENT, USE THE GLOBAL CONTENT MANAGER CONTAINED IN GAME TO LOAD PERSISTENT CONTENT.
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            this.Exit();
        }

        // TODO: UPDATE OUR SERVICES HERE
        for (int i = 0; i < 64; i++)
        {
            engine.updateLightPosition(i, new Vector2(MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * 3, MathF.Cos((float)gameTime.TotalGameTime.TotalSeconds) * 3));
            engine.updateLightPosition(i, new Vector2((float)(rnd.NextDouble() - 0.5) * 3, (float)(rnd.NextDouble() - 0.5) * 3));
        }
        for (int i = 0; i < 32; i++)
        {
            engine.sprites[i].TranslatePosition(new Vector3((float)(rnd.NextDouble() - 0.5) * 2, (float)(rnd.NextDouble() - 0.5) * 2, 0));
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
