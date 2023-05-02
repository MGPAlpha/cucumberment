using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Yarn.Unity;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestLibrary instanceQuestLibrary;
    
    private static QuestLibrary questLibrary;

    private static HashSet<QuestData> completedQuests = new HashSet<QuestData>();
    private static Dictionary<QuestData, int> activeQuests = new Dictionary<QuestData, int>();

    private static HashSet<QuestDialogue> availableQuestDialogues = new HashSet<QuestDialogue>();

    private static HashSet<string> enabledFeatures = new HashSet<string>();
    private static HashSet<JobData> availableJobsFromQuests = new HashSet<JobData>();
    private static HashSet<StationData> revealedStations = new HashSet<StationData>();
    private static HashSet<StationData> goalStations = new HashSet<StationData>();

    public static IEnumerable<QuestDialogue> GetAutoStationDialogues(IEnumerable<CharacterData> characters) {
        return (from dialogue in availableQuestDialogues where characters.Contains(dialogue.character) && dialogue.stationAutomatic select dialogue);
    }

    public static IEnumerable<QuestDialogue> GetCharacterStationDialogues(CharacterData character) {
        return (from dialogue in availableQuestDialogues where dialogue.character == character && !dialogue.inSpace && !dialogue.stationAutomatic select dialogue);
    }

    public static IEnumerable<JobData> GetStationQuestJobs(StationData station) {
        return (from job in availableJobsFromQuests where job.fromStation == station select job);
    }

    public static JobData FindJobByUniqueName(string jobName) {
        print("Available jobs " + availableJobsFromQuests.Count);
        return availableJobsFromQuests.First((job => job.uniqueName == jobName));
    }

    public static bool IsFeatureEnabled(string featureName) {
        return enabledFeatures.Contains(featureName);
    }

    public static bool IsStationGoal(StationData station) {
        return goalStations.Contains(station);
    }

    public static bool IsStationRevealed(StationData station) {
        return revealedStations.Contains(station);
    }

    [SerializeField] private bool inSpace;

    private static bool questsLoaded = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!questLibrary) questLibrary = instanceQuestLibrary;
        if (!questsLoaded) {
            LoadQuests();
            questsLoaded = true;
        }

        UpdateQuestInfo();
    } 
    
    // Start is called before the first frame update
    void Start()
    {
        

        if (inSpace)
            foreach (QuestDialogue dialogue in availableQuestDialogues) {
                if (dialogue.inSpace && dialogue.spaceAutomatic && dialogue.spaceRadius == float.PositiveInfinity) {
                    SpaceDialogueManager.Main.BeginDialogue(dialogue.dialogueNodeName);
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void UpdateQuestInfo() {
        availableQuestDialogues = new HashSet<QuestDialogue>();
        enabledFeatures = new HashSet<string>();
        availableJobsFromQuests = new HashSet<JobData>();
        revealedStations = new HashSet<StationData>();
        goalStations = new HashSet<StationData>();
        foreach (QuestData quest in questLibrary.quests) { // Check all quests

            bool questComplete = completedQuests.Contains(quest);
            bool questActive = activeQuests.ContainsKey(quest);

            if (completedQuests.Contains(quest)) { // Quest is complete

            } else if (activeQuests.ContainsKey(quest)) { // Quest is active
                int questStage = activeQuests[quest];
                QuestStage currentStage = quest.stages[questStage];
                availableQuestDialogues.UnionWith(currentStage.stageDialogues);
            } else { // Quest not started
                if (completedQuests.IsSupersetOf(quest.completeQuestsRequired)) { // Prerequisite quests complete
                    availableQuestDialogues.UnionWith(quest.startDialogue);
                }
            }

            if (questComplete || questActive) {
                for (int i = 0; i < quest.stages.Count; i++) {
                    if (questComplete || i <= activeQuests[quest]) {
                        QuestStage currentStage = quest.stages[i];
                        enabledFeatures.UnionWith(currentStage.enabledFeatures);
                        availableJobsFromQuests.UnionWith(currentStage.stageJobs);
                        revealedStations.UnionWith(currentStage.revealedStations);
                        if (!questComplete && i == activeQuests[quest]) {
                            goalStations.Add(currentStage.goalStation);
                        }
                    }
                }
            }

        }
        Debug.Log("Amount of revealed stations " + revealedStations.Count);

    }

    [YarnCommand("startQuest")]
    public static void StartQuest(string questName) {
        QuestData quest = questLibrary.quests.Find(q => q.questName == questName);
        if (!quest) return;
        if (!activeQuests.ContainsKey(quest) && !completedQuests.Contains(quest)) {
            activeQuests[quest] = 0;
        }
        UpdateQuestInfo();
    }

    [YarnCommand("progressQuest")]
    public static void ProgressQuest(string questName) {
        QuestData quest = questLibrary.quests.Find(q => q.questName == questName);
        if (!quest) return;
        if (activeQuests.ContainsKey(quest) && !completedQuests.Contains(quest)) {
            int questCurrentStage = activeQuests[quest];
            if (questCurrentStage >= quest.stages.Count - 1) {
                activeQuests.Remove(quest);
                completedQuests.Add(quest);
            } else {
                activeQuests[quest]++;
            }
        }
        UpdateQuestInfo();
    }

    private static void LoadQuests() {
        HashSet<string> completedQuestStrings = new HashSet<string>();
        Dictionary<string, int> activeQuestStrings = new Dictionary<string, int>();
        if (PlayerPrefs.HasKey("completedQuests")) {
            completedQuestStrings = JsonConvert.DeserializeObject<HashSet<string>>(PlayerPrefs.GetString("completedQuests"));
        }
        if (PlayerPrefs.HasKey("activeQuests")) {
            activeQuestStrings = JsonConvert.DeserializeObject<Dictionary<string, int>>(PlayerPrefs.GetString("activeQuests"));
        }
        // print("Quest progress " + activeQuestStrings["Game Start"]);
        if (completedQuestStrings.Count > 0 || activeQuestStrings.Count > 0) {
            foreach (QuestData quest in questLibrary.quests) {
                if (completedQuestStrings.Contains(quest.questName)) {
                    completedQuests.Add(quest);
                }
                if (activeQuestStrings.ContainsKey(quest.questName)) {
                    activeQuests[quest] = activeQuestStrings[quest.questName];
                }
            }
        }

    }

    public static void SaveQuests() {
        HashSet<string> completedQuestStrings = new HashSet<string>();
        Dictionary<string, int> activeQuestStrings = new Dictionary<string, int>();
        foreach(QuestData quest in completedQuests) {
            completedQuestStrings.Add(quest.name);
        }
        foreach(QuestData quest in activeQuests.Keys) {
            activeQuestStrings[quest.questName] = activeQuests[quest];
        }
        PlayerPrefs.SetString("completedQuests", JsonConvert.SerializeObject(completedQuestStrings));
        PlayerPrefs.SetString("activeQuests", JsonConvert.SerializeObject(activeQuestStrings));
    }
}
