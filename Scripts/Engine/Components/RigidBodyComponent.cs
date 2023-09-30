namespace LD54.Engine.Components;

using Microsoft.Xna.Framework;

public class RigidBodyComponent : Component
{
    public Vector3 Velocity;
    public float Mass;

    public RigidBodyComponent(string name, Game appCtx) : base(name, appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.Velocity = Vector3.Zero;
    }

    public override void OnUnload()
    {

    }
}
