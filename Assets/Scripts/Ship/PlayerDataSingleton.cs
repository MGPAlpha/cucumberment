using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSingleton : MonoBehaviour
{

    public static ShipCargo Cargo {get; private set;}
    public static FuelManager FuelManager {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<ShipCargo>(out ShipCargo cargoTemp)) {
            Cargo = cargoTemp;
        }
        if (TryGetComponent<FuelManager>(out FuelManager fuelTemp)) {
            FuelManager = fuelTemp;
        }
    }
}
