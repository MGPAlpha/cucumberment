using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class ShipData {
    public FuelTankData fuelTank;
}

[Serializable]
public class FuelTankData {
    public float currentFuel;
    public float density;
    public float efficiency;
}

public class ShipPersistenceController : MonoBehaviour
{
    // [Serializable]
    // public class ShipData {
    //     public FuelTankData fuelTank;
    // }

    // [Serializable]
    // public class FuelTankData {
    //     public float contents;
    //     public float density;
    //     public float efficiency;
    // }

    void Awake()
    {
        LoadShips();
        shipsLoaded = true;
    }

    private static Dictionary<string, ShipData> persistentShips = new Dictionary<string, ShipData>();
    private static bool shipsLoaded = false;
    
    private FuelManager fuelManager;

    private ShipData shipData;

    [SerializeField] private string ShipName = "default";
    
    [SerializeField] private bool persistFuel;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<FuelManager>(out fuelManager);
        if (persistentShips.ContainsKey(ShipName)) {
            shipData = persistentShips[ShipName];
            if (fuelManager && persistFuel && shipData.fuelTank != null) fuelManager.LoadFuelTankData(shipData.fuelTank);
        } else {
            shipData = new ShipData();
            persistentShips[ShipName] = shipData;
            if (fuelManager && persistFuel) {
                shipData.fuelTank = new FuelTankData();
                fuelManager.FillFuelTankData(shipData.fuelTank);
            }
        }
    }

    private void OnDestroy() {
        if (fuelManager && persistFuel) {
            fuelManager.FillFuelTankData(shipData.fuelTank);
        }
        SaveShips();
    }

    public static void SaveShips() {
        
        // PlayerPrefs.SetString("shipNames", JsonUtility.ToJson(new List<string>(persistentShips.Keys)));
        // print(new List<string>(persistentShips.Keys).Count);
        // print(JsonUtility.ToJson(new List<string>(persistentShips.Keys)));
        // foreach (string name in persistentShips.Keys) {
        //     string shipKeyName = "ships." + name;
        //     PlayerPrefs.SetString(shipKeyName, JsonUtility.ToJson(persistentShips[name]));
        // }

        PlayerPrefs.SetString("ships", JsonConvert.SerializeObject(persistentShips));
        
    }

    public static void LoadShips() {
        // if (PlayerPrefs.HasKey("shipNames")) {
        //     List<string> shipNames = JsonUtility.FromJson<List<string>>(PlayerPrefs.GetString("shipNames"));
        //     print(shipNames);
        //     foreach (string name in shipNames) {
        //         string shipKeyName = "ships." + name;
        //         if (PlayerPrefs.HasKey(shipKeyName)) {
        //             ShipData newShip = JsonUtility.FromJson<ShipData>(PlayerPrefs.GetString(shipKeyName));
        //             persistentShips[name] = newShip;
        //         }
        //     }
        // }

        if (PlayerPrefs.HasKey("ships")) {
            Dictionary<string, ShipData> savedShips = JsonConvert.DeserializeObject<Dictionary<string, ShipData>>(PlayerPrefs.GetString("ships"));
            foreach (string name in savedShips.Keys) {
                persistentShips[name] = savedShips[name];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
