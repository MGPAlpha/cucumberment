using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JobData {
    public string uniqueName;
    public string fromStation;
    public string toStation;
    public bool limitedTime;
    public int timeLimit;
    public int pay;
    public List<ItemQuantity> items;

    public QuestData progressQuestOnAccept;

    public int TotalItems { get {
        int count = 0;
        foreach (ItemQuantity item in items) {
            count += item.count;
        }
        return count;
    }}

    public float TotalWeight { get {
        float weight = 0;
        foreach (ItemQuantity item in items) {
            weight += item.Weight;
        }
        return weight;
    }}
}

[Serializable]
public class ItemQuantity {
    public int count;
    public ItemData item;
    public float Weight { get => count * item.weight; }
}

public class JobManager : MonoBehaviour
{
    public static JobData ActiveJob {get; private set;}

    public static void SetActiveJob(JobData job) {
        ActiveJob = job;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
