using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.EditorTools;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<Item,int> itemsInInventory = new Dictionary<Item, int>(); 

    //アイテムを追加する
    public void AddItem(Item Item)
    {
        if(itemsInInventory.ContainsKey(Item))
        {
            itemsInInventory[Item] += 1;
        }
        else
        {
            itemsInInventory.Add(Item,1);
        }
        string str = "";
        foreach (Item key in itemsInInventory.Keys)
        {
            str = str + "key=" + key.name + ",val=" + itemsInInventory[key] + "/";
        }
        Debug.Log(str);
    }
}
