using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Character", fileName = "New Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite icon;
    public string defaultDialogue;
}
