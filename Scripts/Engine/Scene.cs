namespace LD54.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

public abstract class Scene : EngineObject
{
    protected ContentManager contentManager;
    public ContentManager ContentManager { get { return contentManager; } }

    /*protected Dictionary<string, object> */

    protected Scene(string name, Game appCtx) : base(name, appCtx)
    {
        this.contentManager = new ContentManager(this.app.Services);
        this.contentManager.RootDirectory = this.app.Content.RootDirectory;
    }

    /// <summary>
    /// Releases all game resources in this scene.
    /// </summary>
    /// <param name="permanent">If the scene need not be loaded again. (frees more memory)</param>
    protected void UnloadContent(bool permanent=false)
    {
        this.contentManager.Unload();
        if (permanent) this.contentManager.Dispose();
    }
}
