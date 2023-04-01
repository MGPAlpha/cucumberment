using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest Library", fileName = "New Quest Library")]
public class QuestLibrary : ScriptableObject
{
    public List<QuestData> quests;
}
