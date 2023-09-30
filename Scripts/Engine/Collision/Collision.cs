namespace LD54.Engine.Collision;

using System;
using System.Collections.Generic;

public class Collision
{
    public AABB aabb; //AABB of object collided with
    public Overlap overlap;
    public ColliderComponent collider;
    public Collision(AABB aabb, ColliderComponent other, Overlap overlap)
    {
        this.aabb = aabb;
        this.overlap = overlap;
        this.collider = other;
    }
}
