using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Station", menuName = "Stations/Station")]
public class StationData : ScriptableObject
{
    public string displayName = "Station";
    public string sceneName = "StationScene";
}
