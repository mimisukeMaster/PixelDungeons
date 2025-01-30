using UnityEngine;
using TMPro;

//インベントリ内の武器を表示するUI
public class InventoryItem_Weapon : InventoryItem
{
    private int damage;
    private float fireRate;
    private float range;
    private float speed;
    public TMP_Text DamageText;
    public TMP_Text FireRateText;
    public TMP_Text SpeedText;
    public TMP_Text RangeText;

    //初期化
    public void Init(Item_Weapon item_Weapon,InventoryManager inventoryManager,int number,bool isWeapon)
    {
        base.Init(item_Weapon,inventoryManager,number);
        item = item_Weapon;
        this.inventoryManager = inventoryManager;
        //UIを設定
        DamageText.text = item_Weapon.Damage.ToString();
        FireRateText.text = item_Weapon.FireRate.ToString();
        RangeText.text = item_Weapon.Range.ToString();
        SpeedText.text = item_Weapon.Speed.ToString();
    }

    //装備する
    public void Equip(bool isRightHand)
    {
        inventoryManager.ChangeWeapon(isRightHand,(Item_Weapon)item,this,true);
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
