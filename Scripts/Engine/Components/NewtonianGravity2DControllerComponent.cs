namespace LD54.Engine.Components;

using Microsoft.Xna.Framework;

public class NewtonianGravity2DControllerComponent : Component
{
    public float GravitationalConstant { get; protected set; } = 1;

    public RigidBodyComponent Satellites { get; protected set; }

    public NewtonianGravity2DControllerComponent(float gravitationalConstant, string name, Game appCtx) : base(name, appCtx)
    {
        GravitationalConstant = gravitationalConstant;
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
