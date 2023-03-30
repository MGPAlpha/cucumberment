using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDisplay : MonoBehaviour
{
    public static WaypointDisplay Main {get; private set;}

    [SerializeField] private GameObject waypointMarkerPrefab;


    private Dictionary<PositionBroadcaster, WaypointMarker> markers = new Dictionary<PositionBroadcaster, WaypointMarker>();

    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    public void RegisterWaypoint(PositionBroadcaster pos) {
        GameObject newGo = Instantiate(waypointMarkerPrefab, transform.position, Quaternion.identity, this.transform);
        WaypointMarker newMarker = newGo.GetComponent<WaypointMarker>();
        newMarker.Initialize(pos);
        markers[pos] = newMarker;
    }

    public void DeleteWaypoint(PositionBroadcaster pos) {
        WaypointMarker marker = markers[pos];
        Destroy(marker.gameObject);
        markers.Remove(pos);
    }

    public void UpdateWaypointType(PositionBroadcaster pos) {
        
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
