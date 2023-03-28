using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEncumbermentSystem : MonoBehaviour
{
    [SerializeField] private float baseWeight = 20;
    private float currentWeight;

    private IWeightContributor[] weightContributors;

    // Start is called before the first frame update
    void Start()
    {
        weightContributors = GetComponents<IWeightContributor>();
        UpdateWeight();
    }

    private void UpdateWeight() {
        currentWeight = baseWeight;
        foreach (IWeightContributor wc in weightContributors) {
            currentWeight += wc.GetWeight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeight();
    }

    public float SpeedRatio { get => baseWeight / currentWeight; }
}
