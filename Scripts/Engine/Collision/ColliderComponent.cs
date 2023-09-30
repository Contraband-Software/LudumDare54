namespace LD54.Engine.Collision;

using LD54.Engine;
using Microsoft.Xna.Framework;

public class ColliderComponent : Component
{
    public AABB aabb;
    private int colliderID;

    private Vector3 dimensions;
    private Vector3 offset;

    public ColliderComponent(Vector3 dimensions, Vector3 offset, string name, Game appCtx) : base(name, appCtx)
    {
        this.dimensions = dimensions;
        this.offset = offset;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        this.gameObject = parentObject;
        Vector3 colliderOrigin = gameObject.GetGlobalPosition() + offset;
        Vector3 min = new Vector3(colliderOrigin.X, colliderOrigin.Y + dimensions.Y, colliderOrigin.Z);
        Vector3 max = new Vector3(colliderOrigin.X + dimensions.X, dimensions.Y, colliderOrigin.Z);
        this.aabb = new AABB(min, max);

        this.colliderID = this.app.Services.GetService<ICollisionSystemService>().AddColliderToSystem(this);
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ICollisionSystemService>().RemoveColliderFromSystem(colliderID);
    }
}
