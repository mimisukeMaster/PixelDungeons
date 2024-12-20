using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.EditorTools;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<Item,InventoryItem> itemsInInventory = new Dictionary<Item, InventoryItem>();
    [Tooltip("アイテムを表示する個別のUI")]
    public GameObject ItemPanel;
    [Tooltip("武器を表示する個別のUI")]
    public GameObject WeaponPanel;
    [Tooltip("アイテム欄の親")]
    public Transform ItemContent;
    [Tooltip("クリックしたときに表示されるパネル")]
    public GameObject UsePanel;
    [Tooltip("右手")]
    public Transform RightHand;
    [Tooltip("左手")]
    public Transform LeftHand;    

    [Tooltip("デバッグ用初期アイテム")]
    public Item[] startItems;

    private void Start()
    {
        foreach(Item item in startItems)
        {
            AddItem(item,1);
        }
    }

    //アイテムを追加する
    public void AddItem(Item Item,int number)
    {
        if(itemsInInventory.ContainsKey(Item))//キーがすでにある
        {
            itemsInInventory[Item].AddNumber(number);
        }
        else//初めて
        {
            GameObject panel = Instantiate(ItemPanel,ItemContent);
            RectTransform contentPanelRectTransform = ItemContent.GetComponent<RectTransform>();
            contentPanelRectTransform.sizeDelta = new Vector2(contentPanelRectTransform.sizeDelta.x,70 * (itemsInInventory.Count + 1));
            InventoryItem inventoryItem = panel.GetComponent<InventoryItem>();
            inventoryItem.Init(number,Item.name,Item.ItemImage);
            itemsInInventory.Add(Item,inventoryItem);
        }
        string str = "";
        foreach (Item key in itemsInInventory.Keys)
        {
            str = str + "key=" + key.name + ",val=" + itemsInInventory[key] + "/";
        }
        Debug.Log(str);
    }

    public void OpenUsePanel()
    {
        UsePanel.SetActive(true);
    }

    public void CloseUsePanel()
    {
        UsePanel.SetActive(false);
    }

    public void ChangeWeapon(bool isRightHand,GameObject weapon)
    {
        if(isRightHand)
        {
            Destroy(RightHand.GetChild(0));
            Instantiate(weapon,RightHand);
        }
        else
        {
            Destroy(LeftHand.GetChild(0));
            Instantiate(weapon,LeftHand);
        }
    }
}
