global using LD54.Engine;

namespace LD54.AsteroidGame.Scenes;

using Engine.Debug;
using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameScene : Scene
{
    public GameScene(Game appCtx) : base("GameScene", appCtx)
    {

    }

    public override void OnLoad(GameObject? parentObject)
    {
        Texture2D blankTexure = this.contentManager.Load<Texture2D>("Sprites/block");

        DebugPlayer playerBlock = new DebugPlayer(blankTexure, "DebugPlayerController", this.app);
        parentObject.AddChild(playerBlock);

        this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnUnload() => throw new System.NotImplementedException();
}
