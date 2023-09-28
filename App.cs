// Imported across the entire solution
global using System.Diagnostics.CodeAnalysis;       // [DisallowNull]

namespace LD54;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Scripts.Engine;

internal class StartScene : Scene
{
    public StartScene(Game app) : base("firstScene", app)
    {

    }

    public void On()
    {

        // Texture2D image = Content.Load<Texture2D>("Sprites/liberia");
        // Texture2D image = Content.Load<Texture2D>("Sprites/liberia");
        // Texture2D image = Content.Load<Texture2D>("Sprites/liberia");
        // Texture2D image = Content.Load<Texture2D>("Sprites/liberia");
    }
}

public class App : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SceneController sc;

    public App()
    {
        this._graphics = new GraphicsDeviceManager(this);
        this.Content.RootDirectory = "Content";
        this.IsMouseVisible = true;

        sc.AddScene(new StartScene(this));

        this.sc.ChangeScene("startscren");
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
        // TODO: use this.Content to load your game content here

        Texture2D image = Content.Load<Texture2D>("Sprites/liberia");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            this.Exit();
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        this.GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
