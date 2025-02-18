using UnityEngine;
using TMPro;

public class InventoryItem_Consumable : InventoryItem
{
    public TMP_Text hpText;

    public void Init(Item_Armor armor,InventoryManager inventoryManager,int number)
    {
        base.Init(armor,inventoryManager,number);
        hpText.text = armor.MaxHP.ToString();
    }

    public void OnEquip()
    {
        inventoryManager.ChangeArmor((Item_Armor)item,this,true);
    }
}
