using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

[CreateAssetMenu(fileName = "New Station Library", menuName = "Stations/Station Library")]
public class StationLibrary : ScriptableObject
{
    private static StationLibrary _stations;
    
    public static StationLibrary Stations {
        get {
            if (!_stations) {
                _stations = Addressables.LoadAssetAsync<StationLibrary>("Stations").WaitForCompletion();
                Debug.Log(_stations.stations.Count);
            }
            return _stations;
        }
    }

    public List<StationData> stations;

    public StationData FindStationByName(string stationName) {
        return (from station in stations where station.displayName == stationName select station).First(); 
    }
}
