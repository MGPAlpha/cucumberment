using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStageType {
    SpeakToCharacter,
    SpaceDialogue
}

[Serializable]
public class QuestDialogue {
    public string dialogueNodeName;
    public CharacterData character;
    public bool blockNormalDialogue = false;
    public string dialoguePrompt;
    public bool stationAutomatic = false;

    public bool inSpace = false;
    public bool spaceAutomatic = false;
    public Vector3 spaceLocation;
    public float spaceRadius = float.PositiveInfinity;
}

[Serializable]
public class QuestStage {
    public QuestStageType stageType;
    public List<string> enabledFeatures;

    public List<QuestDialogue> stageDialogues;
    public List<JobData> stageJobs;
}

[CreateAssetMenu(menuName = "Quests/Quest", fileName = "New Quest")]
public class QuestData : ScriptableObject
{

    public string questName = "default";
    public List<QuestData> completeQuestsRequired;
    public List<QuestDialogue> startDialogue;
    public List<QuestStage> stages;


}
