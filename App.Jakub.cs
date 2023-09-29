namespace LD54;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class App_Jakub : Game
{
    private readonly GraphicsDeviceManager _graphics;

    // contentManager

    public App_Jakub()
    {
        this._graphics = new GraphicsDeviceManager(this);
        this.Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
    }

    protected override void Initialize() {
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        this.GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }
}

