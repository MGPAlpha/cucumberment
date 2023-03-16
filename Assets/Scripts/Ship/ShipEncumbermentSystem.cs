using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEncumbermentSystem : MonoBehaviour
{
    [SerializeField] private float baseWeight = 20;
    private float currentWeight;


    // Start is called before the first frame update
    void Start()
    {
        UpdateWeight();
    }

    private void UpdateWeight() {
        currentWeight = baseWeight;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeight();
    }

    public float SpeedRatio { get => baseWeight / currentWeight; }
}
