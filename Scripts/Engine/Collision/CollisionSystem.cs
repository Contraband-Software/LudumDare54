/*namespace LD54.Engine.Collision;

using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class CollisionSystem
{
    Game game; 

    List<Sprite> spriteColliders = new List<Sprite>();

    public CollisionSystem(Game game)
    {
        this.game = game;
    }

    public void AddColliderToSystem(Sprite spriteCollider)
    {
        spriteColliders.Add(spriteCollider);
        Debug.WriteLine("Collider added to system. Collider count: " + spriteColliders.Count.ToString());
    }

    public void CollisionUpdate()
    {
        //check collision between each collider

        //for now, if theres an overlap, calculate the overlap and
        //give it back to the sprite that you are checking collision for

        //later, will also need to invoke OnCollisionEnter event on a collider
        foreach(var sprite in spriteColliders)
        {
            CalculateForCollider(sprite);
        }
    }

    /// <summary>
    /// Calculates collisions for given collider
    /// </summary>
    /// <param name="sprite"></param>
    private List<Collision> CalculateForCollider(Sprite sprite)
    {
        AABB a = sprite.aabb;
        List<Collision> collisions = new List<Collision>();
        foreach (var other in spriteColliders)
        {
            if (other == sprite) continue;

            AABB b = other.aabb;
            Overlap collision = TestAABBOverlap(a, b);
            if (collision.isOverlap)
            {
                collisions.Add(new Collision(b, other, collision));
            }
        }
        return collisions;
    }


    /// <summary>
    /// Force a recalculation for a collider
    /// </summary>
    public void RequestCalculation(Vector2 preMovePos, Sprite target) { 
        List<Collision> collisions = CalculateForCollider(target);
        Vector2 postMovePos = target.Position;
        Debug.WriteLine(collisions.Count > 0);
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

            Debug.WriteLine("OverlapX: " + overlapX.ToString());
            Debug.WriteLine("OverlapY: " + overlapY.ToString());

            if(target.Velocity.X != 0 && target.Velocity.Y != 0)
            {
                if (overlapX > overlapY)
                {
                    target.Position.Y -= overlapY * MathF.Sign(target.Velocity.Y);
                }
                else if(overlapX < overlapY)
                {
                    target.Position.X -= overlapX * MathF.Sign(target.Velocity.X);
                }
                else{
                    //if left or right of other object (gapX/combined width > gapY/combined height)
                    float widthTarget = target.aabb.max.X - target.aabb.min.X;
                    float widthCollision = collision.aabb.max.X - collision.aabb.min.X;

                    float gapRatioX = MathF.Abs(collision.collider.Position.X - preMovePos.X) 
                        / (widthTarget + widthCollision);
                        
                    float heightTarget = target.aabb.min.Y - target.aabb.max.Y;
                    float heightCollision = collision.aabb.min.Y - collision.aabb.max.Y;

                    float gapRatioY = MathF.Abs(collision.collider.Position.Y - preMovePos.Y) 
                        / (heightTarget + heightCollision);

                    //on left or right (maintain Y, resolve X)
                    if(gapRatioX > gapRatioY)
                    {
                        target.Position.X -= (overlapX) * MathF.Sign(target.Velocity.X);
                    }
                    //above or below (maintain X, resolve Y)
                    else
                    {
                        target.Position.Y -= (overlapY) * MathF.Sign(target.Velocity.Y);
                    }
                }
            }
            else
            {
                if(overlapX > 0) target.Position.Y -= overlapY * MathF.Sign(target.Velocity.Y);
                if(overlapY > 0) target.Position.X -= overlapX * MathF.Sign(target.Velocity.X);
            }

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
*/