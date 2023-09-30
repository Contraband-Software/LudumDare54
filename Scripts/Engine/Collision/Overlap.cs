namespace LD54.Engine.Collision;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public struct Overlap
{
    public bool isOverlap = false;
    public float[] overlaps = new float[4] { 0, 0, 0, 0 };

    public Overlap(bool isOverlap)
    {
        this.isOverlap = isOverlap;
    }

    public Overlap(bool isOverlap, float[] overlaps)
    {
        this.isOverlap = isOverlap;
        this.overlaps = overlaps;
    }
}

