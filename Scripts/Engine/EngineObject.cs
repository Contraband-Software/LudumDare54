namespace LD54.Engine;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public abstract class EngineObject
{
    public bool Initialized = false;

    protected Game app;

    protected string name;

#pragma warning disable CA1051
    public List<string> Tags = new();
#pragma warning restore CA1051

    public EngineObject(string name, Game appCtx)
    {
        this.name = name;
        this.app = appCtx;

        // this.app.Components.Add(this);
    }

    public string GetName()
    {
        return this.name;
    }

    public abstract void OnLoad(GameObject? parentObject);
    public virtual void Update(GameTime gameTime) { }
    public abstract void OnUnload();
}
