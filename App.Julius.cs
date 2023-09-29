namespace LD54;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Leviathan;

public class App_Julius : Game
{
    private LeviathanEngine engine;
    private Random rnd = new Random();
    // contentManager

    public App_Julius()
    {
        this.Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
        engine = new LeviathanEngine(this);
    }

    protected override void Initialize() {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                engine.AddLight(new Vector2(i * 80, j * 80), new Vector3(i * 100, j * 100, 50));
            }
        }
        //engine.AddLight(new Vector2(150, 150), new Vector3(1000, 1000, 1000));
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Sprite testSprite = new Sprite(this, Matrix.CreateTranslation(new Vector3(i * 120, j * 120, 0)), new Point(100), "Sprites/image", "Sprites/normal");
                Components.Add(testSprite);
                engine.addSprite(testSprite);
            }
        }
        engine.initialize();
        base.Initialize();

        PrintLn("Game initialized");
    }

    protected override void LoadContent()
    {
        engine.loadContent();
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
            engine.sprites[i].translatePosition(new Vector3((float)(rnd.NextDouble() - 0.5) * 2, (float)(rnd.NextDouble() - 0.5) * 2, 0));
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        engine.draw(gameTime);
        base.Draw(gameTime);
    }
}
