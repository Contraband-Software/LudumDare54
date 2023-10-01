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
using Scripts.Engine;

public class JuliusScene : Scene
{
    #region PARAMS
    public const float FORCE_LAW = 2.4f;
    public const float GRAVITATIONAL_CONSTANT = 43f;

    public const float BLACK_HOLE_MASS = 100;   // no need to edit, GRAVITATIONAL_CONSTANT is already directly proportional (Satellites are massless)
    public const int SATELLITES = 100;
    public const float SPEED_MULT = 15f;

    public const float MAP_SIZE = 2f;
    #endregion

    private int sunLight = -1;

    private Texture2D testObjectTexture;

    private LeviathanShader backgroundShader;

    public JuliusScene(Game appCtx) : base("JuliusScene", appCtx)
    {
    }

    public void SpawnAccretionDisk(GameObject parent, Vector2 boundsOffset, Vector2 boundsDimensions, Vector2 blackHole)
    {
        Random rnd = new Random();

        for (int i = 0; i < GameScene.SATELLITES; i++)
        {

            Vector2 startPosition = new Vector2(
                rnd.Next((int)boundsOffset.X, (int)boundsDimensions.X),
                rnd.Next((int)boundsOffset.Y, (int)boundsDimensions.Y));

            Vector2 separation = startPosition - blackHole;
            Vector2 perpendicular = separation.PerpendicularClockwise();
            perpendicular.Normalize();

            // PrintLn(perpendicular.ToString());

            GameObject newSat = new SatelliteObject(
                0,
                new Vector3(perpendicular.X, perpendicular.Y, 0.76f) * GameScene.SPEED_MULT * (1 / MathF.Sqrt(separation.Magnitude())),
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
        ILeviathanEngineService re = this.app.Services.GetService<ILeviathanEngineService>();
        Vector2 windowSize = re.getWindowSize();
        PrintLn("Screen resolution: " + windowSize);

        backgroundShader = new LeviathanShader(this.app, "Shaders/stars");
        backgroundShader.AddParam("strength", 3000);
        backgroundShader.AddParam("blackholeX", 0);
        backgroundShader.AddParam("blackholeY", 0);
        re.bindShader(backgroundShader);

        // simple scene-wide illumination
        sunLight = this.app.Services.GetService<ILeviathanEngineService>().AddLight(new Vector2(200, 200), new Vector3(100000, 100000, 100000));

        // sim set up
        StaticSprite background = new StaticSprite(this.contentManager.Load<Texture2D>("Sprites/nebula"), new Vector2(-500), new Vector2(1000), "background", this.app);
        parentObject.AddChild(background);

        NewtonianSystemObject newtonianSystem = new NewtonianSystemObject(
            GameScene.GRAVITATIONAL_CONSTANT,
            "GravitySimulationObject",
            this.app);
        parentObject.AddChild(newtonianSystem);
        newtonianSystem.SetLocalPosition(new Vector2(0, 0));

        // player controller
        testObjectTexture = this.contentManager.Load<Texture2D>("Sprites/circle");
        DebugPlayer player = new(testObjectTexture, "DebugPlayerController", this.app);
        newtonianSystem.AddChild(player);

        // black hole
        Vector2 blackHolePosition = new Vector2(200, 200);
        GameObject blackHole = new BlackHole(
            BLACK_HOLE_MASS,
            testObjectTexture,
            "BlackHole",
            this.app
            );
        newtonianSystem.AddChild(blackHole);
        blackHole.SetLocalPosition(blackHolePosition);

        // some testing space junk spawning
        SpawnAccretionDisk(newtonianSystem,
            new Vector2(
                windowSize.X * -MAP_SIZE /2,
                windowSize.Y * -MAP_SIZE /2
            ) + blackHolePosition,
            new Vector2(
                windowSize.X * MAP_SIZE,
                windowSize.Y * MAP_SIZE
            ),
            blackHolePosition
        );
    }

    private bool printed = false;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!printed)
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
