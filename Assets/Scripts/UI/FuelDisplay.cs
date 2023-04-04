using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelDisplay : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerDataSingleton.FuelManager) {
            FuelManager fuelManager = PlayerDataSingleton.FuelManager;
            float fuel = fuelManager.CurrentFuel;
            float fuelCapacity = fuelManager.Capacity;
            float normalized = fuel/fuelCapacity;

            slider.value = normalized;
        }
    }
}
