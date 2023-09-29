namespace LD54.Engine;

using GlmNet;

public static class Transform
{
    /// <summary>
    /// Gets a position vector from a matrix
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static vec3 FromMat4(mat4 matrix)
    {
        return new vec3(matrix[3]);
    }
}
