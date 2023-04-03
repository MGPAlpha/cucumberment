using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Library", fileName = "New Item Library")]
public class ItemLibrary : ScriptableObject
{
    
    public List<ItemData> items;

}
