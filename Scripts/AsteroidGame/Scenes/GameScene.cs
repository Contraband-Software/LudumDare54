global using LD54.Engine;

namespace LD54.AsteroidGame.Scenes;

using System;
using System.Collections.Generic;
using Engine.Dev;
using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidGame.GameObjects;
using Engine.Components;
using AsteroidGame.GameObjects;

public class GameScene : Scene
{
    private int sunLight = -1;

    private Texture2D testObjectTexture;

    public GameScene(Game appCtx) : base("GameScene", appCtx)
    {

    }

    public void SpawnAccretionDisk(GameObject parent, Vector2 boundsDimensions, Vector2 blackHole)
    {
        Random rnd = new Random();

        Vector2 startPosition = new Vector2(
            rnd.Next((int)boundsDimensions.X),
            rnd.Next((int)boundsDimensions.Y));

        Vector2 separation = startPosition - blackHole;
        Vector2 perpendicular = separation;

        for (int i = 0; i < 10; i++)
        {
            GameObject newSat = new SatelliteObject(
                0,
                new Vector3(rnd.Next(2), rnd.Next(2), 1),
                testObjectTexture,
                "satelliteObject_" + i,
                this.app
            );

            parent.AddChild(newSat);
            newSat.SetLocalPosition(startPosition);
        }
    }

    public override void OnLoad(GameObject? parentObject)
    {
        Vector2 windowSize = this.app.Services.GetService<ILeviathanEngineService>().getWindowSize();
        PrintLn("Screen resolution: " + windowSize);

        // simple scene-wide illumination
        sunLight = this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(10000000, 10000000, 10000000));

        // sim set up
        NewtonianSystemObject newtonianSystem = new NewtonianSystemObject(
            20,
            "GravitySimulationObject",
            this.app);
        parentObject.AddChild(newtonianSystem);

        // player controller
        testObjectTexture = this.contentManager.Load<Texture2D>("Sprites/block");
        DebugPlayer player = new(testObjectTexture, "DebugPlayerController", this.app);
        newtonianSystem.AddChild(player);

        // black hole
        GameObject blackHole = new BlackHole(
            100,
            testObjectTexture,
            "BlackHole",
            this.app
            );
        newtonianSystem.AddChild(blackHole);
        blackHole.SetLocalPosition(new Vector2(
            windowSize.X / 2,
            windowSize.Y / 2)
        );

        // some testing space junk spawning
        SpawnAccretionDisk(newtonianSystem, windowSize, new Vector2(
            windowSize.X / 2,
            windowSize.Y / 2)
        );
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
