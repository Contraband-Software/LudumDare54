namespace LD54.Engine.Components;

using Microsoft.Xna.Framework;

public class NewtonianGavity2DControllerComponent : Component
{
    public NewtonianGavity2DControllerComponent(string name, Game appCtx) : base(name, appCtx)
    {
    }
    public override void OnLoad(GameObject? parentObject)
    {
        // iterate over children, getting all RigidBody and adding them to a list
    }
    public override void Update(GameTime gameTime)
    {


        base.Update(gameTime);
    }
    public override void OnUnload()
    {
        throw new System.NotImplementedException();
    }
}
