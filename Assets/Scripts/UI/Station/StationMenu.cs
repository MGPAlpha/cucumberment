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

public class StationMenu : MonoBehaviour
{

    [SerializeField] private string currentStation;
    [SerializeField] private GameObject menuItemPrefab;
    [SerializeField] private GameObject menuOptionsPanel;
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private List<CharacterData> availableCharacters;

    private CanvasGroup canvasGroup;

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
        BuildActionMenu();
        canvasGroup.interactable = true;
    }

    void BuildActionMenu() {
        ClearActionMenu();
        if (autoDialogueQueue.Count > 0) {
            QuestDialogue dialogue = autoDialogueQueue.Dequeue();
            TriggerDialogue(dialogue.character, dialogue);
            return;
        }
        foreach (CharacterData character in availableCharacters) {
            CreateMenuItem("Speak to " + character.name, new UnityAction(delegate {
                    TriggerDialogue(character, null);
                }), (RectTransform)menuOptionsPanel.transform);
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


        dialogueRunner.StartDialogue(dialogueName);
        CharacterDisplay.Main.ShowSoloCharacter(character.name);
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
}
