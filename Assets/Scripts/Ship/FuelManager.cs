using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipCore))] 
public class FuelManager : MonoBehaviour, IWeightContributor
{
    private ShipCore shipCore;

    [field: SerializeField] public float Capacity {get; private set;} = 1000; // L
    [SerializeField] private float efficiency = 1; // kJ / Mg
    [SerializeField] private float density = .01f; // Mg/L
    [SerializeField] private float maxBurnRate = 2; // kJ / min

    [field: SerializeField] public float CurrentFuel {get; private set;} = 1000; // L

    public bool CanFly {get => CurrentFuel > 0;}

    // Start is called before the first frame update
    void Start()
    {
        shipCore = GetComponent<ShipCore>();
    }

    public void BurnFuel(float rate, float deltaTime) {
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
}
