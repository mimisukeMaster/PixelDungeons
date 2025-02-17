using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    //インベントリ内にある素材
    public  Dictionary<Item_Material,ItemProperty> materialsInInventory = new Dictionary<Item_Material,ItemProperty>();
    //インベントリ内にある武器
    public  Dictionary<Item_Weapon,ItemProperty> weaponsInInventory = new Dictionary<Item_Weapon,ItemProperty>();
    //インベントリ内にある消費アイテム
    public  Dictionary<Item_Consumable,ItemProperty> consumablesInInventory = new Dictionary<Item_Consumable,ItemProperty>();
    public class ItemProperty
    {
        public Item item;
        public InventoryItem inventoryItem;
        public int number;
        public InventoryManager inventoryManager;

        public ItemProperty(InventoryItem inventoryItem,int number,Item item,InventoryManager inventoryManager)
        {
            this.inventoryItem = inventoryItem;
            this.number = number;
            this.item = item;
            this.inventoryManager = inventoryManager;
        }

        public void AddNumber(int number)
        {
            this.number += number;
            inventoryItem.ChangeText(this.number);
        }

        public void SubNumber(int number)
        {
            this.number -= number;
            inventoryItem.ChangeText(this.number);
            if(this.number <= 0)
            {
                inventoryManager.RemoveItem(item);
                Debug.Log(inventoryItem.gameObject);
                Destroy(inventoryItem.gameObject);
            }
        }

        public bool GetRemovable(int number)
        {
            return this.number >= number;
        }
    }

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
    [Tooltip("初期装備右")]
    public Item_Weapon initialWeaponRight;
    [Tooltip("初期装備左")]
    public Item_Weapon initialWeaponLeft;
    [Tooltip("右手")]
    public Transform RightHand;
    public static Item_Weapon rightHandWeapon = null;
    [Tooltip("左手")]
    public Transform LeftHand;    
    public static Item_Weapon leftHandWeapon = null;
    [Tooltip("右手の攻撃発生場所")]
    public Transform RightEmissionTransform;
    [Tooltip("左手の攻撃発生場所")]
    public Transform LeftEmissionTransform;

    [Tooltip("デバッグ用初期アイテム")]
    public Item[] startItems;
    public static bool hasInitializedInventory = false;

    private void Start()
    {
        if(!hasInitializedInventory)
        {
            foreach(Item item in startItems)
            {
                AddItem(item, 1);
            }
            if(initialWeaponRight != null)
            {
                ChangeWeapon(true, initialWeaponRight, null,false);
            }
            if(initialWeaponLeft != null)
            {
                ChangeWeapon(false, initialWeaponLeft, null,false);
            }
            hasInitializedInventory = true;
        }
        else
        {
            if(initialWeaponRight != null)
            {
                ChangeWeapon(true, initialWeaponRight, null,false);
            }
            if(initialWeaponLeft != null)
            {
                ChangeWeapon(false, initialWeaponLeft, null,false);
            }
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
                InventoryItem inventoryItem = SetPanel(material,panel,number);
                materialsInInventory.Add(material,new ItemProperty((InventoryItem_Material)inventoryItem,1,material,this));
                //UIのサイズを調節する
                SetContentHeight(MaterialUIContent.GetComponent<RectTransform>(),materialsInInventory.Count);
            }
        }
        else if(item is Item_Weapon weapon)
        {
            if(weaponsInInventory.ContainsKey(weapon))
            {
                weaponsInInventory[weapon].AddNumber(number);
            }
            else
            {
                GameObject panel = Instantiate(WeaponPanel,WeaponUIContent);
                //パネル追加
                InventoryItem_Weapon inventoryItem = panel.GetComponent<InventoryItem_Weapon>();
                inventoryItem.Init(weapon,this,number,true);
                weaponsInInventory.Add(weapon,new ItemProperty((InventoryItem_Weapon)inventoryItem,1,weapon,this));
                //UIのサイズを調節する
                SetContentHeight(WeaponUIContent.GetComponent<RectTransform>(),weaponsInInventory.Count);
            }
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
                InventoryItem inventoryItem = SetPanel(consumable,panel,number);
                consumablesInInventory.Add(consumable,new ItemProperty((InventoryItem_Consumable)inventoryItem,1,consumable,this));
                //UIのサイズを調節する
                SetContentHeight(ConsumableUIContent.GetComponent<RectTransform>(),consumablesInInventory.Count);
            }
        }
    }

    //AddNumberの重複を解消する
    private InventoryItem SetPanel(Item item, GameObject panel, int number)
    {
        InventoryItem inventoryItem = panel.GetComponent<InventoryItem>();
        inventoryItem.Init(item,this,number);
        return inventoryItem;
    }

    private void SetContentHeight(RectTransform targetRectTransform,int count)
    {
        targetRectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x, 70 * (count + 1));
    }

    public void RemoveItem(Item item)
    {
        if(item is Item_Material) materialsInInventory.Remove((Item_Material)item);
        else if(item is Item_Weapon)weaponsInInventory.Remove((Item_Weapon)item);
        else if(item is Item_Consumable)consumablesInInventory.Remove((Item_Consumable)item);
    }

    public int GetItemNumber(Item item)
    {
        if(item is Item_Material) return materialsInInventory[(Item_Material)item].number;
        else if(item is Item_Weapon)return weaponsInInventory[(Item_Weapon)item].number;
        else if(item is Item_Consumable)return consumablesInInventory[(Item_Consumable)item].number;
        else return 0;
    }

    public ItemProperty SerchItem(InventoryItem inventoryItem)
    {
        foreach(ItemProperty itemProperty in materialsInInventory.Values)
        {
            if(itemProperty.inventoryItem == inventoryItem){return itemProperty;}
        }
        foreach(ItemProperty itemProperty in weaponsInInventory.Values)
        {
            if(itemProperty.inventoryItem == inventoryItem){return itemProperty;}
        }
        foreach(ItemProperty itemProperty in consumablesInInventory.Values)
        {
            if(itemProperty.inventoryItem == inventoryItem){return itemProperty;}
        }
        
        return null;
    }

    /// <summary>
    /// 装備変更
    /// </summary>
    /// <param name="isRightHand"></param>
    /// <param name="weapon"></param>
    public void ChangeWeapon(bool isRightHand, Item_Weapon weapon, InventoryItem inventoryItem,bool consumeItem)
    {
        if(isRightHand)//右手
        {
            rightHandWeapon = weapon;
            if(RightHand.childCount != 0)
            {
                AddItem(RightHand.GetChild(0).gameObject.GetComponent<WeaponController>().weapon, 1);
                Destroy(RightHand.GetChild(0).gameObject);
            }
            GameObject weaponInstance = Instantiate(weapon.WeaponModelPrefab, RightHand);
            if(inventoryItem != null && consumeItem)
            {
                ItemProperty item = SerchItem(inventoryItem);
                if(item != null)item.SubNumber(1);
            }
            weaponInstance.GetComponent<WeaponController>().Init(weapon, true, RightEmissionTransform);
        }
        else//左手
        {
            leftHandWeapon = weapon;
            if(LeftHand.childCount != 0)
            {
                AddItem(LeftHand.GetChild(0).gameObject.GetComponent<WeaponController>().weapon, 1);
                Destroy(LeftHand.GetChild(0).gameObject);
            }
            GameObject weaponInstance = Instantiate(weapon.WeaponModelPrefab, LeftHand);
            if(inventoryItem != null)
            {
                ItemProperty item = SerchItem(inventoryItem);
                if(item != null)item.SubNumber(1);
            }
            weaponInstance.GetComponent<WeaponController>().Init(weapon, false, LeftEmissionTransform);
        }
    }

    /// <summary>
    /// クラフト
    /// </summary>
    /// <param name="materials"></param>
    /// <param name="number"></param>
    /// <param name="result"></param>
    /// <param name="resultNumber"></param>
    public bool Craft(Item[] materials,int[] number,Item result,int resultNumber)
    {
        //インベントリ内に素材があるかチェックする
        bool[] itemsExists = new bool[materials.Length];
        for(int i = 0;i < materials.Length;i++)
        {
            Item material = materials[i];
            if(material is Item_Material && materialsInInventory.ContainsKey((Item_Material)material))itemsExists[i] = materialsInInventory[(Item_Material)material].GetRemovable(number[i]);
            else if(material is Item_Weapon && weaponsInInventory.ContainsKey((Item_Weapon)material))itemsExists[i] = weaponsInInventory[(Item_Weapon)material].GetRemovable(number[i]);
            else if(material is Item_Consumable &&consumablesInInventory.ContainsKey((Item_Consumable)material))itemsExists[i] = consumablesInInventory[(Item_Consumable)material].GetRemovable(number[i]);
        }

        if(itemsExists.Contains(false))
        {
            Debug.Log("Craft lack material");
            return false;
        }
        
        //素材を消費する
        for(int i = 0;i < materials.Length;i++)
        {
            Item material = materials[i];
            if(material is Item_Material && materialsInInventory.ContainsKey((Item_Material)material)) materialsInInventory[(Item_Material)material].SubNumber(number[i]);
            else if(material is Item_Weapon && weaponsInInventory.ContainsKey((Item_Weapon)material)) weaponsInInventory[(Item_Weapon)material].SubNumber(number[i]);
            else if(material is Item_Consumable && consumablesInInventory.ContainsKey((Item_Consumable)material)) consumablesInInventory[(Item_Consumable)material].SubNumber(number[i]);
        }
        
        AddItem((Item_Weapon)result,resultNumber);
        Debug.Log("Craft Success");
        return true;
    }
}
