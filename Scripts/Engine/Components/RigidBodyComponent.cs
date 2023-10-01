namespace LD54.Engine.Components;

using Microsoft.Xna.Framework;
using System;

public class RigidBodyComponent : Component
{
    public Vector3 Velocity = Vector3.Zero;
    public float Mass = 1;
    public bool Static = false;
    private float maxVelocity = 5f;
    private float velocityDamping = 1f;

    public RigidBodyComponent(string name, Game appCtx) : base(name, appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.gameObject = parentObject;
        maxVelocity = 5f;
    }

    public override void Update(GameTime gameTime)
    {
        if (!this.Enabled) return;

        base.Update(gameTime);

        if (this.Static) return;
        this.gameObject.SetLocalPosition(this.gameObject.GetLocalPosition() + this.Velocity);

        //limit velocity
        if(this.Velocity.Length() > maxVelocity)
        {
            this.Velocity = (this.Velocity/this.Velocity.Length()) * maxVelocity;
        }
        PrintLn(Velocity.ToString());
        this.Velocity *= velocityDamping;
    }

    public override void OnUnload()
    {

    }

    public void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = maxVelocity;
    }
    public void SetDampingFactor(float  dampingFactor)
    {
        this.velocityDamping = dampingFactor;
    }

    /// <summary>
    /// Adds force to the rigidbody in a direction
    /// </summary>
    /// <param name="force">force vector</param>
    public void AddForce(Vector2 force, GameTime gameTime)
    {
        Vector2 acceleration = (force / this.Mass) * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
        this.Velocity += new Vector3(acceleration.X, acceleration.Y, 0);
    }
}
