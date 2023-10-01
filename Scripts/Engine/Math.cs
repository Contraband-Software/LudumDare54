namespace LD54.Scripts.Engine;

using System;
using Microsoft.Xna.Framework;

public static class Math
{
    public static Vector2 PerpendicularClockwise(this Vector2 vector2)
    {
        return new Vector2(vector2.Y, -vector2.X);
    }

    public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
    {
        return new Vector2(-vector2.Y, vector2.X);
    }

    public static float Magnitude(this Vector2 vector2)
    {
        return MathF.Sqrt(MathF.Pow(vector2.X, 2) + MathF.Pow(vector2.Y, 2));
    }
}
