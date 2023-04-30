using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class JobTerminalUI : MonoBehaviour
{

    [SerializeField] private RectTransform jobList;
    [SerializeField] private GameObject jobListItemPrefab;
    [SerializeField] private GameObject jobListCategoryPrefab;
    [SerializeField] private GameObject jobInfoPanel;
    [SerializeField] private GameObject acceptButton;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject deliverButton;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI fromText;
    [SerializeField] private TextMeshProUGUI toText;
    [SerializeField] private TextMeshProUGUI timeLimitText;
    [SerializeField] private TextMeshProUGUI payText;
    [SerializeField] private RectTransform cargoDisplay;
    [SerializeField] private GameObject cargoItemPrefab;
    [SerializeField] private TextMeshProUGUI cargoWeightText;

    [SerializeField] private GameObject deliveryScreen;
    [SerializeField] private TextMeshProUGUI deliveryToText;
    [SerializeField] private TextMeshProUGUI deliveryBasePayText;
    [SerializeField] private TextMeshProUGUI deliveryActualCargoText;
    [SerializeField] private TextMeshProUGUI deliveryExpectedCargoText;
    [SerializeField] private RectTransform deliveryCargoDisplay;
    [SerializeField] private GameObject missingItemsPenaltyBox;
    [SerializeField] private GameObject penaltyPerItemBox;
    [SerializeField] private TextMeshProUGUI penaltyPerItemText;
    [SerializeField] private TextMeshProUGUI totalPayText;

    private StationData station;

    private JobData selectedJob;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void InitializeDisplay(StationData station) {
        this.station = station;
        ClearInfo();
        ClearDelivery();
        BuildJobList();
        gameObject.SetActive(true);
        print("got here");
    }

    public void BuildJobList() {
        // for (int i = jobList.transform.childCount - 1; i >= 0; i--) {
        //     Destroy(jobList.GetChild(i).gameObject);
        // }
        while (jobList.childCount > 0) { // Needs destroyimmediate to not infinitely loop
            DestroyImmediate(jobList.GetChild(0).gameObject);
        }

        if (JobManager.ActiveJob != null) {
            AddCategoryToList("Active Jobs");
            AddJobToList(JobManager.ActiveJob);
        }
        AddCategoryToList("Available Jobs");
        foreach (JobData job in QuestManager.GetStationQuestJobs(station.displayName)) {
            if (job != JobManager.ActiveJob) {
                AddJobToList(job);
            }
        }
    }

    private void AddJobToList(JobData job) {
        JobItem jobUI = Instantiate(jobListItemPrefab, Vector3.zero, Quaternion.identity, jobList.transform).GetComponent<JobItem>();
        Button jobButton = jobUI.GetComponent<Button>();
        jobUI.Init(job);
        jobButton.onClick.AddListener(delegate {LoadJob(job);});
    }

    private void AddCategoryToList(string categoryName) {
        TextMeshProUGUI categoryUI = Instantiate(jobListCategoryPrefab, Vector3.zero, Quaternion.identity, jobList).GetComponent<TextMeshProUGUI>();
        categoryUI.text = categoryName;
    }

    private void TriggerActionError(string error) {
        errorText.text = error;
        errorText.gameObject.SetActive(true);
    }

    private void LoadJob(JobData job) {
        fromText.text = job.fromStation;
        toText.text = job.toStation;
        payText.text = job.pay.ToString();
        cargoWeightText.text = job.TotalWeight.ToString();
        errorText.gameObject.SetActive(false);

        ClearCargoItems(cargoDisplay);
        foreach (ItemQuantity item in job.items) {
            AddCargoItem(item, cargoDisplay);
        }

        acceptButton.SetActive(job != JobManager.ActiveJob);
        cancelButton.SetActive(job == JobManager.ActiveJob);
        deliverButton.SetActive(job == JobManager.ActiveJob && job.toStation == station.displayName);

        selectedJob = job;
        deliveryScreen.SetActive(false);
        jobInfoPanel.SetActive(true);
    }

    private int deliveryActualPay;

    private void OpenDeliveryScreen(JobData job) {
        deliveryToText.text = job.toStation;
        deliveryBasePayText.text = job.pay.ToString();

        int jobPay = job.pay;
        int totalExpectedItems = job.TotalItems;
        int actualItemCount = 0;

        ShipCargo inventory = PlayerDataSingleton.Cargo;

        ClearCargoItems(deliveryCargoDisplay);
        foreach (ItemQuantity item in job.items) {
            actualItemCount += Mathf.Min(item.count, inventory.GetItem(item.item));
        }

        deliveryActualCargoText.text = actualItemCount.ToString();
        deliveryExpectedCargoText.text = totalExpectedItems.ToString();

        foreach (ItemQuantity item in job.items) {
            AddCargoItem(item, deliveryCargoDisplay, true);
        }

        bool itemsMissing = actualItemCount < totalExpectedItems;
        float penaltyPerMissing = -2f/totalExpectedItems;

        missingItemsPenaltyBox.SetActive(itemsMissing);
        penaltyPerItemBox.SetActive(itemsMissing);
        penaltyPerItemText.text = penaltyPerMissing.ToString("P1");

        float penaltyRatio = 1;
        if (itemsMissing) {
            penaltyRatio *= .75f;

            penaltyRatio *= 1 + penaltyPerMissing * (totalExpectedItems - actualItemCount);
        }

        deliveryActualPay = (int)(job.pay * penaltyRatio);
        totalPayText.text = deliveryActualPay.ToString();

        deliveryScreen.SetActive(true);
        jobInfoPanel.SetActive(false);
    }

    public void ConfirmDelivery() {
        ShipCargo cargo = PlayerDataSingleton.Cargo;
        cargo.AddMoney(deliveryActualPay);
        foreach (ItemQuantity item in selectedJob.items) {
            cargo.RemoveItem(item.item, item.count);
        }
        if (selectedJob.progressQuestOnComplete) {
            QuestManager.ProgressQuest(selectedJob.progressQuestOnComplete.questName);
        }
        JobManager.SetActiveJob(null);
        selectedJob = null;
        InitializeDisplay(station);
    }

    public void TryAcceptJob() {
        if (selectedJob == null) return;
        if (JobManager.ActiveJob != null) {
            return; // Log error here later
        }
        if (PlayerDataSingleton.Cargo.OpenSlots < selectedJob.TotalItems) {
            TriggerActionError("Not enough room in your cargo for all these items. Please unload more cargo and try again.");
            return;
        }
        JobManager.SetActiveJob(selectedJob);
        foreach (ItemQuantity item in selectedJob.items) {
            PlayerDataSingleton.Cargo.AddItem(item.item, item.count);
        }
        if (selectedJob.progressQuestOnAccept) QuestManager.ProgressQuest(selectedJob.progressQuestOnAccept.questName);
        InitializeDisplay(station);
    }

    public void TryCancelJob() {
        if (selectedJob == null) return;
        print(selectedJob.uniqueName);
        if (JobManager.ActiveJob != selectedJob) {
            // Maybe log error for trying to cancel inactive job
            return;
        }
        if (selectedJob.uniqueName != "" && selectedJob.uniqueName != null) {
            // Log error cannot cancel job from quest
            TriggerActionError("This job cannot be cancelled. If you wish to restart it, please use the 'Restart' button instead");
            return;
        }
        // dont actually need job cancelling yet but button gives appearance of functionality
    }

    public void TryDeliverJob() {
        if (selectedJob == null) return;
        OpenDeliveryScreen(selectedJob);
        // Trigger delivery option
    }

    private void AddCargoItem(ItemQuantity item, Transform cargoDisplay, bool includeCurrentCount = false) {
        CargoSlot cargoUI = Instantiate(cargoItemPrefab, Vector3.zero, Quaternion.identity, cargoDisplay).GetComponent<CargoSlot>();
        if (includeCurrentCount) {
            cargoUI.Init(item, PlayerDataSingleton.Cargo.GetItem(item.item));
        } else {
            cargoUI.Init(item);
        }
    }

    private void ClearCargoItems(Transform cargoDisplay) {
        while (cargoDisplay.childCount > 0) { // Needs destroyimmediate to not infinitely loop
            DestroyImmediate(cargoDisplay.GetChild(0).gameObject);
        }
    }

    private void ClearInfo() {
        jobInfoPanel.SetActive(false);
    }

    private void ClearDelivery() {
        deliveryScreen.SetActive(false);
    }


    [SerializeField] private UnityEvent OnExit;
    public void ExitTerminal() {
        gameObject.SetActive(false);
        OnExit.Invoke();
    }
}
