using UnityEngine;
using System;

public static class Utilities
{
    public static float Wrap(float v, float min, float max)
    {
        if (v < min) v += Math.Abs(max) + Math.Abs(min);
        if (v > max) v -= Math.Abs(max) + Math.Abs(min);

        return v;
    }

    public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max)
    {
        v.x = Wrap(v.x, min.x, max.x);
        v.y = Wrap(v.y, min.y, max.y);
        v.z = Wrap(v.z, min.z, max.z);

        return v;
    }
}