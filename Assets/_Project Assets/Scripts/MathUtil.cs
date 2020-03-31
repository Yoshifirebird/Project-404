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
}
