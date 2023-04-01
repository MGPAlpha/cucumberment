using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMovable : MonoBehaviour
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        WorldMover.MoveWorld += Move;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        WorldMover.MoveWorld -= Move;
    }

    void Move(Vector3 offset) {
        transform.position += offset;
    }
}
