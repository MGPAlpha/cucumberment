using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerDataSingleton : MonoBehaviour
{

    public static ShipCargo Cargo {get; private set;}
    public static FuelManager FuelManager {get; private set;}
    public static ShipEncumbermentSystem EncumbermentSystem {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<ShipCargo>(out ShipCargo cargoTemp)) {
            Cargo = cargoTemp;
        }
        if (TryGetComponent<FuelManager>(out FuelManager fuelTemp)) {
            FuelManager = fuelTemp;
        }
        if (TryGetComponent<ShipEncumbermentSystem>(out ShipEncumbermentSystem encumbermentTemp)) {
            EncumbermentSystem = encumbermentTemp;
        }
    }

    [YarnCommand("givePlayerMoney")]
    public static void GivePlayerMoney(int amount) {
        Cargo.AddMoney(amount);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
