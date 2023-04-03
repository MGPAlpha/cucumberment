using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class ShipData {
    public FuelTankData fuelTank;
    public CargoData cargoData;
}

[Serializable]
public class FuelTankData {
    public float currentFuel;
    public float density;
    public float efficiency;
}

[Serializable]
public class CargoData {
    public Dictionary<string, int> inventory;
    public int capacity;
    public int money;
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
        if (!shipsLoaded) {
            LoadShips();
            shipsLoaded = true;
        }
    }

    private static Action backupActiveShips;

    private static Dictionary<string, ShipData> persistentShips = new Dictionary<string, ShipData>();
    private static bool shipsLoaded = false;
    
    private FuelManager fuelManager;
    private ShipCargo shipCargo;

    private ShipData shipData;

    [SerializeField] private string ShipName = "default";
    
    [SerializeField] private bool persistFuel;
    [SerializeField] private bool persistCargo;

    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        // Debug.Log(persistentShips["Player"].fuelTank.currentFuel);
        TryGetComponent<FuelManager>(out fuelManager);
        TryGetComponent<ShipCargo>(out shipCargo);
        if (persistentShips.ContainsKey(ShipName)) {
            shipData = persistentShips[ShipName];
            if (fuelManager && persistFuel) {
                if (shipData.fuelTank != null) fuelManager.LoadFuelTankData(shipData.fuelTank);
                else {
                    shipData.fuelTank = new FuelTankData();
                    fuelManager.FillFuelTankData(shipData.fuelTank);
                }
            }
            if (shipCargo && persistCargo) {
                if (shipData.cargoData != null) shipCargo.LoadCargoData(shipData.cargoData);
                else {
                    shipData.cargoData = new CargoData();
                    shipCargo.FillCargoData(shipData.cargoData);
                }
            }
        } else {
            shipData = new ShipData();
            persistentShips[ShipName] = shipData;
            if (fuelManager && persistFuel) {
                shipData.fuelTank = new FuelTankData();
                fuelManager.FillFuelTankData(shipData.fuelTank);
            }
            if (shipCargo && persistCargo) {
                shipData.cargoData = new CargoData();
                shipCargo.FillCargoData(shipData.cargoData);
            }
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        backupActiveShips += BackupToShipData;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        backupActiveShips -= BackupToShipData;
    }

    private void BackupToShipData() {
        if (fuelManager && persistFuel) {
            fuelManager.FillFuelTankData(shipData.fuelTank);
            print("Current fuel on destroy" + shipData.fuelTank.currentFuel);
        }
        if (shipCargo && persistCargo) {
            shipCargo.FillCargoData(shipData.cargoData);
        }
    }


    public static void SaveShips() {
        
        // PlayerPrefs.SetString("shipNames", JsonUtility.ToJson(new List<string>(persistentShips.Keys)));
        // print(new List<string>(persistentShips.Keys).Count);
        // print(JsonUtility.ToJson(new List<string>(persistentShips.Keys)));
        // foreach (string name in persistentShips.Keys) {
        //     string shipKeyName = "ships." + name;
        //     PlayerPrefs.SetString(shipKeyName, JsonUtility.ToJson(persistentShips[name]));
        // }

        backupActiveShips.Invoke();
        Debug.Log(persistentShips["Player"].fuelTank.currentFuel);
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
                print("ship " + name + " fuel " + savedShips[name].fuelTank.currentFuel);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
