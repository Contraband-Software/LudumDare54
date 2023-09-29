// Imported across the entire solution
global using System.Diagnostics.CodeAnalysis;       // [DisallowNull]
global using static LD54.Engine.Debug;

namespace LD54;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine;



class OtherPlayer : Player
{
    private ISceneControllerService sc;

    // private Game app;

    public OtherPlayer(string name, Game appCtx) : base(name, appCtx)
    {
        this.sc = appCtx.Services.GetService<ISceneControllerService>();

        // this.app = appCtx;
    }

    public override void OnLoad(GameObject parentObject)
    {
        // PrintLn("Onload: " + " " + parentObject.GetName() + " " + this.GetName());
    }
}

class Player : GameObject
{
    public Player(string name, Game appCtx) : base(name, appCtx)
    {
        ISceneControllerService sc = appCtx.Services.GetService<ISceneControllerService>();
    }

    public override void OnLoad(GameObject parentObject)
    {
        PrintLn("Onload: " + this.GetName());

        var gc = new OtherPlayer("PEEVIS", this.app);
        this.AddChild(gc);
    }

    public override void OnUnload()
    {
        PrintLn("i is kill");
    }
}


internal class StartScene : Scene
{
    public StartScene(Game app) : base("firstScene", app)
    {

    }

    public override void OnLoad(GameObject parentObject)
    {
        // content loading

        // scene graph population
        // parentObject = root game object in this context

        //player = some shit

        PrintLn("loading start scene");

        var gc = new Player("player", this.app);
        parentObject.AddChild(gc);

        this.app.Services.GetService<ISceneControllerService>().GetPersistentGameObject().AddChild(new Player("persistent player", this.app));
    }

    public override void Update(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.Seconds % 4 == 0)
        {
            PrintLn("scene doing thing");
        }
    }

    public override void OnUnload()
    {
        PrintLn("start scene gone");
        this.UnloadContent();
    }
}

internal class OtherScene : Scene
{
    public OtherScene(Game appCtx) : base("otherscene", appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        PrintLn("new scene--------------------------------");
    }

    public override void OnUnload()
    {

    }
}

/*
 *persitant game object init
 *root gameobject onload run
 * switch to xna math
 *
 */






public class App : Game
{
    private readonly GraphicsDeviceManager _graphics;
    SceneController sc;

    private int changedScene = 0;

    // contentManager

    public App()
    {
        this._graphics = new GraphicsDeviceManager(this);
        this.Content.RootDirectory = "Content";
        this.IsMouseVisible = true;

        PrintLn("hello world");
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here

        base.Initialize();

        sc = new SceneController(this);
        this.Components.Add(sc);
        this.Services.AddService(typeof(ISceneControllerService), sc);

        var startscene = new StartScene(this);
        this.sc.AddScene(startscene);
        var otherscene = new OtherScene(this);
        this.sc.AddScene(otherscene);
        this.sc.ChangeScene("firstScene");

        PrintLn("Game initialized");
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        // GLOBAL LOAD CONTENT, USE THE GLOBAL CONTENT MANAGER CONTAINED IN GAME TO LOAD PERSISTENT CONTENT.
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            this.Exit();
        }

        // TODO: Add your update logic here

        base.Update(gameTime);

        this.sc.Update(gameTime);

        if (gameTime.TotalGameTime.Seconds % 5 == 0)
        {
            this.sc.DebugPrintGraph();
        }

        if (gameTime.TotalGameTime.Seconds > 5 && changedScene == 0)
        {
            PrintLn("change scene");
            this.changedScene++;
            this.sc.ChangeScene("otherscene");
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        this.GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
