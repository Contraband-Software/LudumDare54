global using LD54.Engine;

namespace LD54.AsteroidGame.Scenes;

using System;
using Engine.Dev;
using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidGame.GameObjects;
using Engine.Components;
using Scripts.AsteroidGame.GameObjects;

public class GameScene : Scene
{
    private int sunLight = -1;

    public GameScene(Game appCtx) : base("GameScene", appCtx)
    {

    }

    public override void OnLoad(GameObject? parentObject)
    {
        // simple scene-wide illumination
        sunLight = this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));

        NewtonianSystemObject newtonianSystem = new NewtonianSystemObject(1,"GravitySimulationObject", this.app);
        parentObject.AddChild(newtonianSystem);

        Texture2D blankTexure = this.contentManager.Load<Texture2D>("Sprites/block");
        DebugPlayer player = new(blankTexure, "DebugPlayerController", this.app);
        newtonianSystem.AddChild(player);

        // this.app.Services.GetService<ILeviathanEngineService>().

        // Random rnd = new Random();
        // for (int i = 0; i < 10; i++)
        // {
        //     GameObject newSat = new SatelliteObject(
        //         10,
        //         new Vector3(rnd.Next(2), rnd.Next(2), 1),
        //         blankTexure,
        //         "satelliteObject_" + i,
        //         this.app
        //         );
        //     newtonianSystem.AddChild(newSat);
        // }
    }

    private bool printed = false;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (gameTime.TotalGameTime.Seconds > 1 && !printed)
        {
            printed = true;
            this.app.Services.GetService<ISceneControllerService>().DebugPrintGraph();
        }
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ILeviathanEngineService>().removeLight(this.sunLight);
    }
}
