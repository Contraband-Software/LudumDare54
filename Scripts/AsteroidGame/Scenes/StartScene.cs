namespace LD54.AsteroidGame.Scenes;

using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Engine;
using GameObjects;
using Microsoft.Xna.Framework.Graphics;

public class StartScene : Scene
{
    private readonly int showTime = 0;

    private ILeviathanEngineService? render;
    private ISceneControllerService? scene;

    public StartScene(int showTime, Game appCtx) : base("StartScene", appCtx)
    {
        this.showTime = showTime;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        render = this.app.Services.GetService<ILeviathanEngineService>();
        scene = this.app.Services.GetService<ISceneControllerService>();

        GameObject titleUI = new TitleScreenSystem(
            this.showTime,
            this.contentManager.Load<SpriteFont>("Fonts/TitleFont"),
            this.contentManager.Load<SpriteFont>("Fonts/SubtitleFont"),
            this.app
            );

        parentObject.AddChild(titleUI);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnUnload()
    {

    }
}
