using UnityEngine;

public static class Utilities
{
    public static float Wrap(float v, float min, float max)
    {
        if (v < min) v += max;
        if (v > max) v -= max;
        
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