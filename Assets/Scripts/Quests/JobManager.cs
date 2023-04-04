using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class JobData {
    public JobData(JobDataSerializable job) {
        if (job != null) {
            items = new List<ItemQuantity>();
            return;
        };
        uniqueName = job.uniqueName;
        fromStation = job.fromStation;
        toStation = job.toStation;
        limitedTime = job.limitedTime;
        timeLimit = job.timeLimit;
        pay = job.pay;

        items = job.items.ConvertAll<ItemQuantity>(item => new ItemQuantity(item));
    }

    public string uniqueName;
    public string fromStation;
    public string toStation;
    public bool limitedTime;
    public int timeLimit;
    public int pay;
    public List<ItemQuantity> items;

    public QuestData progressQuestOnAccept;
    public QuestData progressQuestOnComplete;

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
public class JobDataSerializable {

    public JobDataSerializable() {}
    public JobDataSerializable(JobData job) {
        if (job == null) return;
        uniqueName = job.uniqueName;
        fromStation = job.fromStation;
        toStation = job.toStation;
        limitedTime = job.limitedTime;
        timeLimit = job.timeLimit;
        pay = job.pay;

        items = job.items.ConvertAll<ItemQuantitySerializable>(item => new ItemQuantitySerializable(item));
    }

    public string uniqueName;
    public string fromStation;
    public string toStation;
    public bool limitedTime;
    public int timeLimit;
    public int pay;
    public List<ItemQuantitySerializable> items;
}

[Serializable]
public class ItemQuantity {
    public ItemQuantity(ItemQuantitySerializable item) {
        count = item.count;
        this.item = ShipCargo.ItemLibrary.items.Find((i) => i.itemName == item.itemName);
    }
    public int count;
    public ItemData item;
    public float Weight { get => count * item.weight; }
}
[Serializable]
public class ItemQuantitySerializable {
    public ItemQuantitySerializable() {}
    public ItemQuantitySerializable(ItemQuantity item) {
        count = item.count;
        itemName = item.item.itemName;
    }
    public int count;
    public string itemName;
}

public class JobManager : MonoBehaviour
{
    
    public static JobData ActiveJob {get; private set;}
    private static bool jobsLoaded = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!jobsLoaded) {
            LoadJobs();
            jobsLoaded = true;
        }
    }

    public static void SetActiveJob(JobData job) {
        ActiveJob = job;
    }
    
    public static void SaveJobs() {
        JobDataSerializable job = new JobDataSerializable(ActiveJob);
        PlayerPrefs.SetString("activeJob", JsonConvert.SerializeObject(job));
    }

    public static void LoadJobs() {
        if (PlayerPrefs.HasKey("activeJob")) {
            JobDataSerializable stringJob = JsonConvert.DeserializeObject<JobDataSerializable>(PlayerPrefs.GetString("activeJob"));
            print(stringJob.uniqueName);
            if (stringJob == null || (stringJob.pay == 0 && stringJob.timeLimit == 0)) {
                ActiveJob = null;
            } else if (stringJob.uniqueName != null && stringJob.uniqueName != "null") {
                
                print(stringJob.uniqueName);
                ActiveJob = QuestManager.FindJobByUniqueName(stringJob.uniqueName);
            } else {
                ActiveJob = new JobData(stringJob);
            }
        } else {
            ActiveJob = null;
        }
    }

    
}
