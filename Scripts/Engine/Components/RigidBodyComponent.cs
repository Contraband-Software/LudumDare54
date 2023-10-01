namespace LD54.Engine.Components;

using Microsoft.Xna.Framework;
using System;

public class RigidBodyComponent : Component
{
    public Vector3 Velocity = Vector3.Zero;
    public float Mass = 1;
    public bool Static = false;

    public RigidBodyComponent(string name, Game appCtx) : base(name, appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.gameObject = parentObject;
    }

    public override void Update(GameTime gameTime)
    {
        if (!this.Enabled) return;

        base.Update(gameTime);

        if (this.Static) return;
        this.gameObject.SetLocalPosition(this.gameObject.GetLocalPosition() + this.Velocity);
    }

    public override void OnUnload()
    {

    }

    /// <summary>
    /// Adds velocity based off of the direction the gameobject is facing
    /// </summary>
    public void AddVelocityForward(Vector2 velocity)
    {
        this.Velocity = new Vector3(velocity.X, velocity.Y, 0f);
    }
}
