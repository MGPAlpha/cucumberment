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

    private string station;

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



    public void InitializeDisplay(string station) {
        this.station = station;
        ClearInfo();
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
        foreach (JobData job in QuestManager.GetStationQuestJobs(station)) {
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

    private void LoadJob(JobData job) {
        fromText.text = job.fromStation;
        toText.text = job.toStation;
        payText.text = job.pay.ToString();
        cargoWeightText.text = job.TotalWeight.ToString();
        errorText.gameObject.SetActive(false);

        ClearCargoItems();
        foreach (ItemQuantity item in job.items) {
            AddCargoItem(item);
        }

        acceptButton.SetActive(job != JobManager.ActiveJob);
        cancelButton.SetActive(job == JobManager.ActiveJob);
        deliverButton.SetActive(job == JobManager.ActiveJob && job.toStation == station);

        selectedJob = job;
        jobInfoPanel.SetActive(true);
    }

    public void TryAcceptJob() {
        if (selectedJob == null) return;
        if (JobManager.ActiveJob != null) {
            return; // Log error here later
        }
        JobManager.SetActiveJob(selectedJob);
        if (selectedJob.progressQuestOnAccept) QuestManager.ProgressQuest(selectedJob.progressQuestOnAccept.questName);
        InitializeDisplay(station);
    }

    public void TryCancelJob() {
        if (selectedJob == null) return;
        if (JobManager.ActiveJob != selectedJob) {
            // Maybe log error for trying to cancel inactive job
            return;
        }
        if (selectedJob.uniqueName != "" && selectedJob.uniqueName != null) {
            // Log error cannot cancel job from quest
        }
        // dont actually need job cancelling yet but button gives appearance of functionality
    }

    public void TryDeliverJob() {
        if (selectedJob == null) return;
        
        // Trigger delivery option
    }

    private void AddCargoItem(ItemQuantity item) {
        CargoSlot cargoUI = Instantiate(cargoItemPrefab, Vector3.zero, Quaternion.identity, cargoDisplay).GetComponent<CargoSlot>();
        cargoUI.Init(item);
    }

    private void ClearCargoItems() {
        while (cargoDisplay.childCount > 0) { // Needs destroyimmediate to not infinitely loop
            DestroyImmediate(cargoDisplay.GetChild(0).gameObject);
        }
    }

    private void ClearInfo() {
        jobInfoPanel.SetActive(false);
    }


    [SerializeField] private UnityEvent OnExit;
    public void ExitTerminal() {
        gameObject.SetActive(false);
        OnExit.Invoke();
    }
}
