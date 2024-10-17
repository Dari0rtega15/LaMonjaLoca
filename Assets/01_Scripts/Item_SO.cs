using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "ScriptableObjects/Item")]
public class Item_SO : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public ItemCategory category;
    public Sprite sprite;

    public enum ItemCategory
    {
        Heal,
        Damage,
    }
}
