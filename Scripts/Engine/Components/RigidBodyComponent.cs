namespace LD54.Engine.Components;

using Microsoft.Xna.Framework;

public class RigidBodyComponent : Component
{
    public Vector3 Velocity = Vector3.Zero;
    public float Mass = 1;

    public RigidBodyComponent(string name, Game appCtx) : base(name, appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.gameObject = parentObject;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        this.gameObject.SetLocalPosition(this.gameObject.GetLocalPosition() + this.Velocity);
    }

    public override void OnUnload()
    {

    }
}
