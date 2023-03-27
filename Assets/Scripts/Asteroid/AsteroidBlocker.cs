using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBlocker : MonoBehaviour
{

    [field: SerializeField] public float Radius {get; private set;} = 100;
    [field: SerializeField] public float InnerRadius {get; private set;} = 75;

    private static List<AsteroidBlocker> activeBlockers = new List<AsteroidBlocker>();

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        activeBlockers.Add(this);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        activeBlockers.Remove(this);
    }

    public static bool TestAsteroidBlock(Vector3 pos) {
        foreach (AsteroidBlocker blo in activeBlockers) {
            float distToBlocker = (pos-blo.transform.position).magnitude;
            if (distToBlocker < blo.InnerRadius) return true;
            else if (Random.Range(blo.InnerRadius, blo.Radius) > distToBlocker) return true;
        }
        return false;
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.DrawWireSphere(transform.position, InnerRadius);
    }
}
