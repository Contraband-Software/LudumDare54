global using LD54.Engine;

namespace LD54.AsteroidGame.Scenes;

using Engine.Dev;
using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidGame.GameObjects;
using Scripts.AsteroidGame.GameObjects;

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

        // simple scene-wide illumination
        sunLight = this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));

        NewtonianSystemObject newtonianSystem = new NewtonianSystemObject("GravitySimulationObject", this.app);
        parentObject.AddChild(newtonianSystem);

        newtonianSystem.AddChild(player);

        for (int i = 0; i < 10; i++)
        {
            // GameObject newSat = new SatelliteObject(blankTexure, "satelliteObject_" + i, this.app);
            // newSat.SetLocalPosition(new Vector2(10, 10));
            // newtonianSystem.AddChild(newSat);
        }

        this.app.Services.GetService<ISceneControllerService>().DebugPrintGraph();
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
