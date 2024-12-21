using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using System.Globalization;

public class InventoryManager : MonoBehaviour
{
    //インベントリ内にある素材
    public Dictionary<Item_Material,InventoryItem_Material> materialsInInventory = new Dictionary<Item_Material,InventoryItem_Material>();
    //インベントリ内にある武器
    public Dictionary<Item_Weapon,InventoryItem_Weapon> weaponsInInventory = new Dictionary<Item_Weapon,InventoryItem_Weapon>();
    //インベントリ内にある消費アイテム
    public Dictionary<Item_Consumable,InventoryItem_Consumable> consumablesInInventory = new Dictionary<Item_Consumable,InventoryItem_Consumable>();

    [Tooltip("アイテムを表示する個別のUIのプレハブ")]
    public GameObject MaterialPanel;
    [Tooltip("武器を表示する個別のUIのプレハブ")]
    public GameObject WeaponPanel;
    [Tooltip("消費アイテムを表示するUIのプレハブ")]
    public GameObject ConsumablePanel;

    [Tooltip("素材アイテム欄の親")]
    public Transform MaterialUIContent;
    [Tooltip("武器アイテム欄の親")]
    public Transform WeaponUIContent;
    [Tooltip("消費アイテム欄の親")]
    public Transform ConsumableUIContent;
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
    public void AddItem(Item item,int number)
    {
        if(item is Item_Material material)
        {
            if(materialsInInventory.ContainsKey(material))//キーがすでにある
            {
                materialsInInventory[material].AddNumber(number);
            }
            else//初めて
            {
                GameObject panel = Instantiate(MaterialPanel,MaterialUIContent);
                //パネル追加
                InventoryItem_Material inventoryItem = panel.GetComponent<InventoryItem_Material>();
                inventoryItem.Init(material.name,material.ItemImage,number);
                materialsInInventory.Add(material,inventoryItem);
                //UIのサイズを調節する
                RectTransform contentPanelRectTransform = MaterialUIContent.GetComponent<RectTransform>();
                contentPanelRectTransform.sizeDelta = new Vector2(contentPanelRectTransform.sizeDelta.x,70 * (materialsInInventory.Count + 1));
            }
        }
        else if(item is Item_Weapon weapon)
        {
            GameObject panel = Instantiate(WeaponPanel,WeaponUIContent);
            //パネル追加
            InventoryItem_Weapon inventoryItem = panel.GetComponent<InventoryItem_Weapon>();
            inventoryItem.Init(weapon.name,weapon.ItemImage,weapon.Damage,weapon.FireRate,weapon.Range,weapon.Speed);
            weaponsInInventory.Add(weapon,inventoryItem);
            //UIのサイズを調節する
            RectTransform contentPanelRectTransform = WeaponUIContent.GetComponent<RectTransform>();
            contentPanelRectTransform.sizeDelta = new Vector2(contentPanelRectTransform.sizeDelta.x,70 * (weaponsInInventory.Count + 1));
        }
        else if(item is Item_Consumable consumable)
        {
            if(consumablesInInventory.ContainsKey(consumable))//キーがすでにある
            {
                consumablesInInventory[consumable].AddNumber(number);
            }
            else//初めて
            {
                GameObject panel = Instantiate(ConsumablePanel,ConsumableUIContent);
                //パネル追加
                InventoryItem_Consumable inventoryItem = panel.GetComponent<InventoryItem_Consumable>();
                inventoryItem.Init(item.name,item.ItemImage,number);
                consumablesInInventory.Add(consumable,inventoryItem);
                //UIのサイズを調節する
                RectTransform contentPanelRectTransform = ConsumableUIContent.GetComponent<RectTransform>();
                contentPanelRectTransform.sizeDelta = new Vector2(contentPanelRectTransform.sizeDelta.x,70 * (consumablesInInventory.Count + 1));
            }
        }

        string str = "";
        foreach (Item_Material key in materialsInInventory.Keys)
        {
            str = str + "key=" + key.name + ",val=" + materialsInInventory[key] + "/";
        }
        foreach (Item_Weapon key in weaponsInInventory.Keys)
        {
            str = str + "key=" + key.name + ",val=" + weaponsInInventory[key] + "/";
        }
        foreach (Item_Consumable key in consumablesInInventory.Keys)
        {
            str = str + "key=" + key.name + ",val=" + consumablesInInventory[key] + "/";
        }
        Debug.Log(str);
    }

    //AddNumberの重複を解消する
    private void SetUI(Item item)
    {

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
