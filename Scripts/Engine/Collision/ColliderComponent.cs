namespace LD54.Engine.Collision;

using LD54.Engine;
using LD54.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

public class ColliderComponent : Component
{
    public AABB aabb;
    private int colliderID;

    private Vector3 dimensions;
    private Vector3 offset;

    public Vector3 previousPosition;

    public ColliderComponent(Vector3 dimensions, Vector3 offset, string name, Game appCtx) : base(name, appCtx)
    {
        this.dimensions = dimensions;
        this.offset = offset;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.gameObject = parentObject;
        RecalculateAABB();

        this.colliderID = this.app.Services.GetService<ICollisionSystemService>().AddColliderToSystem(this);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        RigidBodyComponent rb = (RigidBodyComponent)gameObject.GetComponent<RigidBodyComponent>();
        if (rb != null)
        {
            previousPosition = (gameObject.GetGlobalPosition() + offset) - rb.Velocity;
        }
        else
        {
            previousPosition = gameObject.GetGlobalPosition() + offset;
        }

        //update aabb per update to match where gameObject is
        RecalculateAABB();

        //draw outline of collider
    }

    public void RecalculateAABB()
    {
        Vector3 colliderOrigin = gameObject.GetGlobalPosition() + offset;
        Vector3 min = new Vector3(colliderOrigin.X, colliderOrigin.Y + dimensions.Y, colliderOrigin.Z);
        Vector3 max = new Vector3(colliderOrigin.X + dimensions.X, colliderOrigin.Y, colliderOrigin.Z);
        this.aabb.min = min;
        this.aabb.max = max;
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ICollisionSystemService>().RemoveColliderFromSystem(colliderID);
    }

    public GameObject? GetGameObject()
    {
        return this.gameObject;
    }
}
