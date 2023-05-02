using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointType {
    Station,
    Target,
    Enemy
}

public class PositionBroadcaster : MonoBehaviour
{
    
    [SerializeField] private WaypointType waypointType;
    public WaypointType WaypointType {
        get => waypointType;
        private set {
            waypointType = value;
            WaypointDisplay.Main.UpdateWaypointType(this);
        }
    }

    [field: SerializeField] public StationData Station {get; private set;}

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        WaypointDisplay.Main.RegisterWaypoint(this);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        WaypointDisplay.Main.DeleteWaypoint(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
