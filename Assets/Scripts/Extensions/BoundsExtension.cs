using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtension
{

    public static Vector3 RandomPoint(this Bounds b) {
        Vector3 min = b.min;
        Vector3 max = b.max;

        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }

}
