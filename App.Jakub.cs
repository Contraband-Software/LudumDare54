namespace LD54;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine;
using Microsoft.Xna.Framework.Graphics;

class PlayerBlock : GameObject
{
    public PlayerBlock(string name, Game appCtx) : base(name, appCtx)
    {

    }

    public override void OnLoad(GameObject? parentObject)
    {

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}

class JakubScene : Scene
{
    public JakubScene(Game appCtx) : base("JakubScene", appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        Texture2D blankTexure = this.contentManager.Load<Texture2D>("Sprites/block");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnUnload() => throw new System.NotImplementedException();
}


public class App_Jakub : Game
{
    private GraphicsDeviceManager graphics;
    private SceneController sc;

    public App_Jakub()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        sc = new SceneController(this);
        this.Components.Add(sc);
        this.Services.AddService(typeof(ISceneControllerService), sc);
        sc.AddScene(new JakubScene(this));
        sc.ChangeScene("JakubScene");

        base.Initialize();
    }

    protected override void LoadContent()
    {
/*        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var _playerTexture = Content.Load<Texture2D>("block");
        _sprites = new List<Sprite>()
            {
                new Player(_playerTexture, this)
                {
                    Position = new Vector2(0,0),
                    Speed = 5f,
                    _color = Color.Blue,
                    Scale = new Vector2(1,1),
                    name = "Player"
                },

                new Sprite(_playerTexture, this)
                {
                    Position = new Vector2(300,100),
                    _color = Color.White,
                    Scale = new Vector2(1,1)
                },
                new Sprite(_playerTexture, this)
                {
                    Position = new Vector2(364,100),
                    _color = Color.White,
                    Scale = new Vector2(1,1)
                }
            };*/
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        collisionSystem.CollisionUpdate();
        foreach (var sprite in _sprites)
        {
            sprite.Update(gameTime, _sprites);
        }*/
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
/*        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        foreach (var sprite in _sprites)
        {
            sprite.Draw(_spriteBatch);

        }


        _spriteBatch.End();
        // TODO: Add your drawing code here
*/
        base.Draw(gameTime);
    }
}
}

