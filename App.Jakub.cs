namespace LD54;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using LD54.Engine.Leviathan;

class SpriteRendererComponent : Component
{
    LeviathanSprite sprite;
    int spriteID;

    public SpriteRendererComponent(string name, Game appCtx) : base(name, appCtx)
    {

    }

    public void LoadSpriteData(Matrix transform, Point size, Texture2D colorTexture, Texture2D? normalTexture = null)
    {
        sprite = new(this.app, transform, size, colorTexture, normalTexture);

        spriteID = this.app.Services.GetService<ILeviathanEngineService>().addSprite(sprite);

        this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));

        PrintLn("LoadSpriteData");
    }

    public override void OnLoad(GameObject? parentObject)
    {
        gameObject = parentObject;
        PrintLn("OnLoad: SpriteRendererComponent");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        sprite.SetTransform(gameObject.GetGlobalTransform());
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ILeviathanEngineService>().removeSprite(spriteID);
        PrintLn("OnUnload: SpriteRendererComponent");
    }
}


class PlayerBlock : GameObject
{
    Texture2D texture;
    public Vector2 Velocity;
    public float Speed;
    public PlayerBlock(Texture2D texture, string name, Game appCtx) : base(name, appCtx)
    {
        this.texture = texture;

        Matrix pos = this.GetLocalTransform();
        pos.Translation = new Vector3(250, 250, 1);

        this.SetLocalTransform(pos);
    }

    public override void OnLoad(GameObject? parentObject)
    {
        SpriteRendererComponent src = new SpriteRendererComponent("Sprite1", this.app);
        src.LoadSpriteData(
            parentObject.GetGlobalTransform(),
            new Point(this.texture.Width, this.texture.Height),
            this.texture,
            null);

        this.AddComponent(src);
        PrintLn("OnLoad: PlayerBlock");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            Velocity.X -= Speed;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            Velocity.X += Speed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            Velocity.Y -= Speed;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            Velocity.Y += Speed;
        }
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
        PrintLn("OnLoad: JakubScene");

        PlayerBlock playerBlock = new PlayerBlock(blankTexure, "spovus", app);
        parentObject.AddChild(playerBlock);
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
    private LeviathanEngine le;

    public App_Jakub()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        EngineDebug.PrintLn("RUNNING APP_HAKUB");

    }

    protected override void Initialize()
    {
        le = new LeviathanEngine(this);
        this.Components.Add(le);
        this.Services.AddService(typeof(ILeviathanEngineService), le);


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

