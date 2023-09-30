namespace LD54;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using LD54.Engine.Leviathan;
using LD54.Engine.Collision;
using Engine.Components;





class LevelBlock : GameObject
{
    Texture2D texture;
    public LevelBlock(Texture2D texture, string name, Game appCtx) : base(name, appCtx)
    {
        this.texture = texture;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        SpriteRendererComponent src = new SpriteRendererComponent("texture", this.app);
        src.LoadSpriteData(
            this.GetGlobalTransform(),
            new Point(this.texture.Width, this.texture.Height),
            this.texture,
            null);

        this.AddComponent(src);

        Vector3 colliderDimensions = new Vector3(this.texture.Width, this.texture.Height, 0);
        ColliderComponent collider = new ColliderComponent(colliderDimensions, Vector3.Zero, "playerCollider", this.app);
        this.AddComponent(collider);
    }
}


class PlayerBlock : GameObject
{
    Texture2D texture;
    public float Speed = 5f;
    ColliderComponent collider;
    RigidBodyComponent rb;
    public PlayerBlock(Texture2D texture, string name, Game appCtx) : base(name, appCtx)
    {
        this.texture = texture;

        Matrix pos = this.GetLocalTransform();
        pos.Translation = new Vector3(150, 150, 1);

        this.SetLocalTransform(pos);
    }

    public override void OnLoad(GameObject? parentObject)
    {
        SpriteRendererComponent src = new SpriteRendererComponent("Sprite1", this.app);
        src.LoadSpriteData(
            this.GetGlobalTransform(),
            new Point(this.texture.Width, this.texture.Height),
            this.texture,
            null);

        this.AddComponent(src);

        Vector3 colliderDimensions = new Vector3(this.texture.Width, this.texture.Height, 0);
        collider = new ColliderComponent(colliderDimensions, Vector3.Zero, "playerCollider", this.app);
        this.AddComponent(collider);

        rb = new RigidBodyComponent("rbPlayer", app);
        this.AddComponent(rb);

        PrintLn("OnLoad: PlayerBlock");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        rb.Velocity = Vector3.Zero;
        Vector3 preMovePosition = this.GetLocalPosition();

        Move();

        this.SetLocalPosition(preMovePosition + rb.Velocity);
        this.app.Services.GetService<ICollisionSystemService>().RequestCalculation(preMovePosition, collider);
    }


    private void Move()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            rb.Velocity.X -= Speed;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            rb.Velocity.X += Speed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            rb.Velocity.Y -= Speed;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            rb.Velocity.Y += Speed;
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

        LevelBlock levelBlock = new LevelBlock(blankTexure, "spovus", app);
        levelBlock.SetLocalPosition(new Vector3(300, 300, 1));
        parentObject.AddChild(levelBlock);

        this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));
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
    private CollisionSystem cs;

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

        cs = new CollisionSystem(this);
        this.Components.Add(cs);
        this.Services.AddService(typeof(ICollisionSystemService), cs);


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

