namespace LD54.Engine.Collision;

using LD54.Engine;
using LD54.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

public class CircleColliderComponent : ColliderComponent
{
    public Vector2 centre; //this is the equivalent of the aabb
    public float radius;

    public CircleColliderComponent(float radius, string name, Game appCtx) : base(name, appCtx)
    {
        this.radius = radius;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        base.OnLoad(parentObject);
        RecalculateCentre();
        
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        RigidBodyComponent rb = (RigidBodyComponent)gameObject.GetComponent<RigidBodyComponent>();
        if (rb != null)
        {
            previousPosition = (gameObject.GetGlobalPosition()) - rb.Velocity;
        }
        else
        {
            previousPosition = gameObject.GetGlobalPosition();
        }
        RecalculateCentre();
    }

    public void RecalculateCentre()
    {
        Vector3 colliderOrigin = gameObject.GetGlobalPosition();
        centre = new Vector2(colliderOrigin.X + radius, colliderOrigin.Y + radius);
    }
}

public class BoxColliderComponent : ColliderComponent
{
    public AABB aabb;
    private Vector3 dimensions;
    private Vector3 offset;

    public BoxColliderComponent(Vector3 dimensions, Vector3 offset, string name, Game appCtx) : base(name, appCtx)
    {
        this.dimensions = dimensions;
        this.offset = offset;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        base.OnLoad(parentObject);
        RecalculateAABB();
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
    }

    public void RecalculateAABB()
    {
        Vector3 colliderOrigin = gameObject.GetGlobalPosition() + offset;
        Vector3 min = new Vector3(colliderOrigin.X, colliderOrigin.Y + dimensions.Y, colliderOrigin.Z);
        Vector3 max = new Vector3(colliderOrigin.X + dimensions.X, colliderOrigin.Y, colliderOrigin.Z);
        this.aabb.min = min;
        this.aabb.max = max;
    }

}

public abstract class ColliderComponent : Component
{
    private int colliderID;
    public Vector3 previousPosition;
    public bool isTrigger = false;

    public ColliderComponent(string name, Game appCtx) : base(name, appCtx)
    {
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.gameObject = parentObject;
        this.colliderID = this.app.Services.GetService<ICollisionSystemService>().AddColliderToSystem(this);
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
