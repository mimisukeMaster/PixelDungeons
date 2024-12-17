using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.EditorTools;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<Item,InventoryItem> itemsInInventory = new Dictionary<Item, InventoryItem>();
    [Tooltip("アイテムを表示する個別のUI")]
    public GameObject ItemPanel;
    [Tooltip("アイテム欄の親")]
    public Transform ItemContent;

    //アイテムを追加する
    public void AddItem(Item Item)
    {
        if(itemsInInventory.ContainsKey(Item))//キーがすでにある
        {
            itemsInInventory[Item].AddNumber(1);
        }
        else//初めて
        {
            GameObject panel = Instantiate(ItemPanel,ItemContent);
            InventoryItem inventoryItem = panel.GetComponent<InventoryItem>();
            inventoryItem.Init(1,Item.name,Item.ItemImage);
            itemsInInventory.Add(Item,inventoryItem);
        }
        string str = "";
        foreach (Item key in itemsInInventory.Keys)
        {
            str = str + "key=" + key.name + ",val=" + itemsInInventory[key] + "/";
        }
        Debug.Log(str);
    }
}
