using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Yarn.Unity;
using TMPro;

public enum MenuMode {
    MainMenu,
    CharacterDialogue
}

public class StationMenu : MonoBehaviour
{

    [SerializeField] private string currentStation;
    [SerializeField] private GameObject menuItemPrefab;
    [SerializeField] private GameObject menuOptionsPanel;
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private JobTerminalUI jobTerminal;
    [SerializeField] private FuelTerminal fuelTerminal;

    [SerializeField] private List<CharacterData> availableCharacters;

    private CanvasGroup canvasGroup;

    private MenuMode menuMode = MenuMode.MainMenu;
    private CharacterData selectedCharacter;

    private Queue<QuestDialogue> autoDialogueQueue = new Queue<QuestDialogue>();
    private HashSet<QuestDialogue> queuedAutoDialogues = new HashSet<QuestDialogue>();

    // Start is called before the first frame update
    void Start()
    {
        LoadAutoDialogues();
        canvasGroup = GetComponent<CanvasGroup>();
        Cursor.lockState = CursorLockMode.None;
        BuildActionMenu();
    }

    private void LoadAutoDialogues() {
        HashSet<QuestDialogue> autoDialogues = new HashSet<QuestDialogue>(QuestManager.GetAutoStationDialogues(availableCharacters));
        print(autoDialogues.Count);
        foreach (QuestDialogue dialogue in (from d in autoDialogues where !queuedAutoDialogues.Contains(d) select d)) {
            autoDialogueQueue.Enqueue(dialogue);
            queuedAutoDialogues.Add(dialogue);
        }
    }

    public void ReturnControlToMenu() {
        LoadAutoDialogues();
        CharacterDisplay.Main.HideDisplay();
        if (menuMode == MenuMode.CharacterDialogue) BuildCharacterMenu(selectedCharacter);
        else BuildActionMenu();
        canvasGroup.interactable = true;
    }

    void BuildActionMenu() {
        ClearActionMenu();
        menuMode = MenuMode.MainMenu;
        if (autoDialogueQueue.Count > 0) {
            QuestDialogue dialogue = autoDialogueQueue.Dequeue();
            TriggerDialogue(dialogue.character, dialogue);
            return;
        }

        CreateMenuItemIfFeature("Access VernaCo Terminal", delegate {
            jobTerminal.InitializeDisplay(currentStation);
            GiveUpMenuControl();
        }, (RectTransform)menuOptionsPanel.transform, "jobTerminal");

        CreateMenuItemIfFeature("Buy Fuel", delegate {
            fuelTerminal.OpenFuelScreen();
            GiveUpMenuControl();
        }, (RectTransform)menuOptionsPanel.transform, "buyFuel");

        foreach (CharacterData character in availableCharacters) {
            CreateMenuItem("Speak to " + character.name, new UnityAction(delegate {
                    BuildCharacterMenu(character);
                }), (RectTransform)menuOptionsPanel.transform);
        }
    }

    void BuildCharacterMenu(CharacterData character) {
        ClearActionMenu();
        menuMode = MenuMode.CharacterDialogue;
        selectedCharacter = character;
        CreateMenuItem("Back", new UnityAction(delegate {
                BuildActionMenu();
                CharacterDisplay.Main.HideDisplay();
            }), (RectTransform)menuOptionsPanel.transform);
        CreateMenuItem("Make Small Talk", new UnityAction(delegate {
                TriggerDialogue(character, null);
            }), (RectTransform)menuOptionsPanel.transform);
        foreach (QuestDialogue dialogue in QuestManager.GetCharacterStationDialogues(character)) {
            CreateMenuItem(dialogue.dialoguePrompt, new UnityAction(delegate {
                    TriggerDialogue(character, dialogue);
                }), (RectTransform)menuOptionsPanel.transform);
        }
        
        if (character.icon) {
            CharacterDisplay.Main.ShowSoloCharacter(character.name);
        }

    }

    void ClearActionMenu() {
        int childCount = menuOptionsPanel.transform.childCount;
        for (int i = childCount-1; i >= 0; i--) {
            Destroy(menuOptionsPanel.transform.GetChild(i).gameObject);
        }
    }

    private void TriggerDialogue(CharacterData character, QuestDialogue dialogue) {
        string dialogueName;
        if (dialogue == null) dialogueName = character.defaultDialogue;
        else dialogueName = dialogue.dialogueNodeName;

        if (!character) character = dialogue.character;

        if (character.icon) {
            CharacterDisplay.Main.ShowCharacterOne("Umbra");
            CharacterDisplay.Main.ShowCharacterTwo(character.name);
        }
        dialogueRunner.StartDialogue(dialogueName);
        
        GiveUpMenuControl();
    }

    private void GiveUpMenuControl() {
        canvasGroup.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveStation() {
        DockingField.SetCurrentStation(currentStation, "", false);
        SaveSystem.SaveGame();
        SceneManager.LoadScene("Space");
    }

    private void CreateMenuItem(string text, UnityAction onClick, RectTransform parent) {
        GameObject newMenuItem = Instantiate(menuItemPrefab, Vector3.zero, Quaternion.identity, parent);
        Button newButton = newMenuItem.GetComponent<Button>();
        TextMeshProUGUI itemText = newMenuItem.GetComponentInChildren<TextMeshProUGUI>();
        itemText.text = text;
        newButton.onClick.AddListener(onClick);
    }

    private void CreateMenuItemIfFeature(string text, UnityAction onClick, RectTransform parent, string featureName) {
        if (QuestManager.IsFeatureEnabled(featureName)) {
            CreateMenuItem(text, onClick, parent);
        }
    }
}
