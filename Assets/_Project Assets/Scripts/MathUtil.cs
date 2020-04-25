using UnityEngine;

public static class MathUtil
{
    public const float M_2PI = Mathf.PI * 2;

    /// <summary>
    /// Calculates the relative position of an object at an index in a circle
    /// </summary>
    /// <param name="segments">How many segments there are (quality)</param>
    /// <param name="index">The index of the object to calculate the position of</param>
    /// <returns>The relative position (-1 to 1, -1 to 1)</returns>
    public static Vector2 CalcPosInCirc(uint segments, int index)
    {
        float theta = (M_2PI / segments) * index;
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }

    /// <summary>
    /// Calculates the relative position of an object at an index in a circle with an added offset
    /// </summary>
    /// <param name="segments">How many segments there are (quality)</param>
    /// <param name="index">The index of the object to calculate the position of</param>
    /// <param name="offset">How much to offset the calculation by</param>
    /// <returns>The relative position (-1 to 1, -1 to 1)</returns>
    public static Vector2 CalcPosInCirc(uint segments, int index, float offset)
    {
        float theta = (M_2PI / segments) * index;
        return new Vector2(Mathf.Cos(theta + offset), Mathf.Sin(theta + offset));
    }

    /// <summary>
    /// Converts between 2D and 3D on the X and Z axis
    /// </summary>
    /// <param name="conv">The vector to convert</param>
    /// <returns>Vector3 with X and Z set to the X and Y of the Vector2</returns>
    public static Vector3 _2Dto3D(Vector2 conv, float y = 0) => new Vector3(conv.x, y, conv.y);

    public static float DistanceTo(Vector3 first, Vector3 second, bool useY = true)
    {
        float xD = first.x - second.x;
        float yD = useY ? first.y - second.y : 0;
        float zD = first.z - second.z;
        return xD * xD + yD * yD + zD * zD;
    }
}
