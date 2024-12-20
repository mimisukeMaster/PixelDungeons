using UnityEngine;

//インベントリ上の武器の挙動を書くところ
//WeaponControllerとは違うので注意
public class Item_Weapon : Item
{
    private int damage;
    private float fireRate;
    private float speed;
    private float range;

    public void Init(int damage,float fireRate,float speed, float range)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        this.speed = speed;
        this.range = range;
    }

    
}
