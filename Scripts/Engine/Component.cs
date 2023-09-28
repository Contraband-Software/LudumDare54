namespace LD54.Scripts.Engine;

using Microsoft.Xna.Framework;

public abstract class Component : EngineObject
{
    public Component(string name) : base(name) { }

    public abstract void OnLoad(GameObject parentObject);
    public abstract void OnUpdate(GameTime gameTime);
    public abstract void OnUnload();
}
