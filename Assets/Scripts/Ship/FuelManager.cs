using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
 
public class FuelManager : MonoBehaviour, IWeightContributor
{


    [field: SerializeField] public float Capacity {get; private set;} = 1000; // L
    [SerializeField] private float efficiency = 1; // kJ / Mg
    [SerializeField] private float density = .01f; // Mg/L
    [SerializeField] private float maxBurnRate = 2; // kJ / min

    [field: SerializeField] public float CurrentFuel {get; private set;} = 1000; // L

    public bool CanFly {get => CurrentFuel > 0;}


    private bool ignoreBurn = false;

    [YarnCommand("ignoreFuelBurn")]
    public void IgnoreBurn(bool val) {
        ignoreBurn = val;
    }

    public void BurnFuel(float rate, float deltaTime) {
        if (ignoreBurn) return;
        float actualBurnRate = maxBurnRate/60;

        float kj = actualBurnRate * deltaTime * rate;
        float mass = kj / efficiency;
        float volume = mass / density;

        CurrentFuel -= volume;
        CurrentFuel = Mathf.Max(CurrentFuel, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetWeight() {
        return CurrentFuel * density;
    }

    public void LoadFuelTankData(FuelTankData data) {
        efficiency = data.efficiency;
        CurrentFuel = data.currentFuel;
        density = data.density;    
    }

    public void FillFuelTankData(FuelTankData data) {
        data.efficiency = efficiency;
        data.currentFuel = CurrentFuel;
        data.density = density;
    }
}
