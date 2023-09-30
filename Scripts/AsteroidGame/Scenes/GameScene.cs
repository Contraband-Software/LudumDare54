global using LD54.Engine;

namespace LD54.AsteroidGame.Scenes;

using Engine.Debug;
using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameScene : Scene
{
    private int sunLight = -1;

    public GameScene(Game appCtx) : base("GameScene", appCtx)
    {

    }

    public override void OnLoad(GameObject? parentObject)
    {
        Texture2D blankTexure = this.contentManager.Load<Texture2D>("Sprites/block");

        DebugPlayer player = new(blankTexure, "DebugPlayerController", this.app);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        parentObject.AddChild(player);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        sunLight = this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ILeviathanEngineService>().removeLight(this.sunLight);
    }
}
