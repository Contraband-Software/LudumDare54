namespace LD54.Scripts.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public abstract class Scene : EngineObject
{
    protected Game app;
    protected ContentManager? content;

    protected Scene(string name, Game app) : base(name)
    {
        this.app = app;
    }

    public virtual void Initialize()
    {
        this.content = new ContentManager(this.app.Services);
        this.content.RootDirectory = this.app.Content.RootDirectory;

        this.LoadContent();
    }

    public virtual void LoadContent() { }

    public virtual void UnloadContent()
    {
        this.content?.Unload();
        this.content = null;
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch) { }
}
