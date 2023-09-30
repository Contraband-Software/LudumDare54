namespace LD54.Engine.Collision;

using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


public struct AABB
{
    public Vector2 min { get; set; }
    public Vector2 max { get; set; }

    public AABB(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }


}

