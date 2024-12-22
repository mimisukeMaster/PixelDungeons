using UnityEngine;

//インベントリ内の武器を表示するUI
public class InventoryItem_Weapon : InventoryItem
{
    private int damage;
    private float fireRate;
    private float range;
    private float speed;

    private GameObject prefab;
    private InventoryManager inventoryManager;
    private Item_Weapon item_Weapon;

    //初期化
    public void Init(Item_Weapon item_Weapon,InventoryManager inventoryManager)
    {
        base.Init(item_Weapon.name, item_Weapon.ItemImage);
        this.item_Weapon = item_Weapon;
        damage = item_Weapon.Damage;
        fireRate = item_Weapon.FireRate;
        range = item_Weapon.Range;
        speed = item_Weapon.Speed;
        prefab = item_Weapon.Prefab;
        this.inventoryManager = inventoryManager;
    }

    //装備する
    public void Equip(bool isRightHand)
    {
        inventoryManager.ChangeWeapon(isRightHand,item_Weapon);
    }

    public void OnEquipRightClick()//右手
    {
        Equip(true);
    }

    public void OnEquipLeftClick()//左手
    {
        Equip(false);
    }
}
