using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Character Library", fileName = "New Character Library")]
public class CharacterLibrary : ScriptableObject
{
    
    public List<CharacterData> characters;

}
