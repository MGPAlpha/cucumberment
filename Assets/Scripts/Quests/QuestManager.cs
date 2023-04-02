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

    public static IEnumerable<QuestDialogue> GetAutoStationDialogues(IEnumerable<CharacterData> characters) {
        return (from dialogue in availableQuestDialogues where characters.Contains(dialogue.character) && dialogue.stationAutomatic select dialogue);
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
    } 
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateQuestInfo();
        

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
        foreach (QuestData quest in questLibrary.quests) { // Check all quests

            if (completedQuests.Contains(quest)) { // Quest is complete

            } else if (activeQuests.ContainsKey(quest)) { // Quest is active
                int questStage = activeQuests[quest];
                availableQuestDialogues.UnionWith(quest.stages[questStage].stageDialogues);
            } else { // Quest not started
                if (completedQuests.IsSupersetOf(quest.completeQuestsRequired)) { // Prerequisite quests complete
                    availableQuestDialogues.UnionWith(quest.startDialogue);
                }
            }

        }


        print("avalialbe dialogues: " + availableQuestDialogues.Count + " " + availableQuestDialogues);
    }

    [YarnCommand("startQuest")]
    private static void StartQuest(string questName) {
        QuestData quest = questLibrary.quests.Find(q => q.questName == questName);
        if (!quest) return;
        if (!activeQuests.ContainsKey(quest) && !completedQuests.Contains(quest)) {
            activeQuests[quest] = 0;
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
