namespace LD54.AsteroidGame.Scenes;

using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Engine;
using GameObjects;
using Microsoft.Xna.Framework.Graphics;

public class StartScene : Scene
{
    private readonly float showTime = 0;
    private float timeShowed = 0;

    private ILeviathanEngineService? render;
    private ISceneControllerService? scene;

    public StartScene(float showTime, Game appCtx) : base("StartScene", appCtx)
    {
        this.showTime = showTime;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        render = this.app.Services.GetService<ILeviathanEngineService>();
        scene = this.app.Services.GetService<ISceneControllerService>();

        GameObject titleUI = new TitleScreenSystem(this.contentManager.Load<SpriteFont>("Fonts/TitleFont"), this.app);
        parentObject.AddChild(titleUI);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        timeShowed += gameTime.TotalGameTime.Seconds;

        float increment = this.showTime / 4;

        // if (this.timeShowed > increment && !showedTitle)
        // {
        //     showedTitle = true;
        // } else if (this.timeShowed > increment * 2 && !showedStudio)
        // {
        //     showedStudio = true;
        //
        // } else if (this.timeShowed > increment * 3 && !showedEngine)
        // {
        //     showedEngine = true;
        //
        // } else if (this.timeShowed > increment * 4)
        // {
        //     this.scene.ChangeScene("GameScene");
        // }
        this.scene.ChangeScene("GameScene");
    }

    public override void OnUnload()
    {

    }
}
