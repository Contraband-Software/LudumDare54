namespace LD54.Scripts.AsteroidGame.GameObjects;

using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NewtonianSystemObject : GameObject
{
    public NewtonianSystemObject(string name, Game appCtx) : base(name, appCtx)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnLoad(GameObject? parentObject)
    {

    }

    // expose method to be called when all children are set
}
