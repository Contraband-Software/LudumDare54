global using LD54.Engine;

namespace LD54.AsteroidGame.Scenes;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Engine.Dev;
using Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidGame.GameObjects;
using Engine.Components;
using AsteroidGame.GameObjects;
using Engine;
using LD54.AsteroidGame.GameObjects;

public class StartScene : Scene
{
    private readonly float showTime = 0;
    private float timeShowed = 0;

    private ILeviathanEngineService render;
    private ISceneControllerService scene;

    public StartScene(float showTime, Game appCtx) : base("StartScene", appCtx)
    {
        this.showTime = showTime;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        render = this.app.Services.GetService<ILeviathanEngineService>();
        scene = this.app.Services.GetService<ISceneControllerService>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        timeShowed += gameTime.TotalGameTime.Seconds;

        if (this.timeShowed > this.showTime)
        {
            this.scene.ChangeScene("GameScene");
        }
    }

    public override void OnUnload()
    {

    }
}
