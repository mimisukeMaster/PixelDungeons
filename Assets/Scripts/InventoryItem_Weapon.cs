using UnityEngine;

public class InventoryItem_Weapon : InventoryItem
{
    private int damage;
    private float fireRate;
    private float range;
    private float speed;

    public void Init(string name, Sprite image,int damage,float fireRate,float range,float speed)
    {
        base.Init(name, image);
        
    }
}
