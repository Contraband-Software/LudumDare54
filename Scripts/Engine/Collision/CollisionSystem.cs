namespace LD54.Engine.Collision;

using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public interface ICollisionSystemService
{
    public int AddColliderToSystem(ColliderComponent spriteCollider);
    public void RemoveColliderFromSystem(int spriteColliderID);

    public void RequestCalculation(Vector3 preMovePos, ColliderComponent requestingCollider);
}

public class CollisionSystem : GameComponent, ICollisionSystemService
{
    private List<ColliderComponent> collisionSystemList;

    public CollisionSystem(Game game) : base(game)
    {
        collisionSystemList = new List<ColliderComponent>();
    }

    public int AddColliderToSystem(ColliderComponent spriteCollider)
    {
        collisionSystemList.Add(spriteCollider);
        PrintLn("Collider added, count: " + collisionSystemList.Count.ToString());
        return collisionSystemList.IndexOf(spriteCollider);
    }

    public void RemoveColliderFromSystem(int spriteColliderID)
    {
        collisionSystemList.RemoveAt(spriteColliderID);
    }



    public void CollisionUpdate()
    {
        //check collision between each collider

        //for now, if theres an overlap, calculate the overlap and
        //give it back to the sprite that you are checking collision for

        //later, will also need to invoke OnCollisionEnter event on a collider
        /*foreach(ColliderComponent col in collisionSystemList)
        {
            CalculateForCollider(col);
        }*/
    }

    /// <summary>
    /// Calculates collisions for given collider
    /// </summary>
    /// <param name="collider"></param>
    private List<Collision> CalculateForCollider(ColliderComponent collider)
    {
        AABB a = collider.aabb;
        List<Collision> collisions = new List<Collision>();
        foreach (ColliderComponent other in collisionSystemList)
        {
            if (other == collider) continue;

            AABB b = other.aabb;
            Overlap collision = TestAABBOverlap(a, b);
            if (collision.isOverlap)
            {
                PrintLn(collision.overlaps.ToString());
                collisions.Add(new Collision(b, other, collision));
            }
        }
        return collisions;
    }


    /// <summary>
    /// Force a recalculation for a collider
    /// </summary>
    public void RequestCalculation(Vector3 preMovePos, ColliderComponent requestingCollider) { 
        List<Collision> collisions = CalculateForCollider(requestingCollider);

        GameObject requestingColliderObj = requestingCollider.GetGameObject();
        RigidBodyComponent requestingColliderRb = (RigidBodyComponent)requestingColliderObj.GetComponent<RigidBodyComponent>();
        Vector3 position = requestingColliderObj.GetGlobalTransform().Translation;


        foreach(Collision collision in collisions)
        {
            //resolve collision
            //overlap box is smaller of d1/2x, d1/2y
            float overlapX = MathF.Min(
                MathF.Abs(collision.overlap.overlaps[0]), 
                MathF.Abs(collision.overlap.overlaps[2]));
            float overlapY = MathF.Min(
                MathF.Abs(collision.overlap.overlaps[1]), 
                MathF.Abs(collision.overlap.overlaps[3]));

            //Debug.WriteLine("OverlapX: " + overlapX.ToString());
            //Debug.WriteLine("OverlapY: " + overlapY.ToString());

            if(requestingColliderRb.Velocity.X != 0 && requestingColliderRb.Velocity.Y != 0)
            {
                if (overlapX > overlapY)
                {
                    position.Y -= overlapY * MathF.Sign(position.Y);
                }
                else if(overlapX < overlapY)
                {
                    position.X -= overlapX * MathF.Sign(position.X);
                }
                else{
                    //if left or right of other object (gapX/combined width > gapY/combined height)
                    float widthTarget = requestingCollider.aabb.max.X - requestingCollider.aabb.min.X;
                    float widthCollision = collision.aabb.max.X - collision.aabb.min.X;

                    float gapRatioX = MathF.Abs(collision.collider.aabb.min.X - preMovePos.X) 
                        / (widthTarget + widthCollision);
                        
                    float heightTarget = requestingCollider.aabb.min.Y - requestingCollider.aabb.max.Y;
                    float heightCollision = collision.aabb.min.Y - collision.aabb.max.Y;

                    float gapRatioY = MathF.Abs(collision.collider.aabb.max.Y - preMovePos.Y) 
                        / (heightTarget + heightCollision);

                    //on left or right (maintain Y, resolve X)
                    if(gapRatioX > gapRatioY)
                    {
                        position.X -= (overlapX) * MathF.Sign(position.X);
                    }
                    //above or below (maintain X, resolve Y)
                    else
                    {
                        position.Y -= (overlapY) * MathF.Sign(position.Y);
                    }
                }
            }
            else
            {
                if(overlapX > 0) position.Y -= overlapY * MathF.Sign(position.Y);
                if(overlapY > 0) position.X -= overlapX * MathF.Sign(position.X);
            }

            requestingCollider.GetGameObject().SetLocalPosition(position);

        }
    }

    /// <summary>
    /// Return whether two aabb's are overlapping
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private Overlap TestAABBOverlap(AABB a, AABB b)
    {
        float d1x = b.min.X - a.max.X;
        float d1y = b.min.Y - a.max.Y;
        float d2x = a.min.X - b.max.X;
        float d2y = a.min.Y - b.max.Y;

        if (d1x > 0.0f || d1y < 0.0f)
        {
            return new Overlap(false);
        }

        if (d2x > 0.0f || d2y < 0.0f)
        {
            return new Overlap(false);
        }

        return new Overlap(true, new float[4] { d1x, d1y, d2x, d2y });
    }
}

