using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{

    public static float ComponentProduct(this Vector3 vec) {
        return vec.x * vec.y * vec.z;
    }

}
