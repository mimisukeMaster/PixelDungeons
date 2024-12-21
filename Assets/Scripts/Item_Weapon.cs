using UnityEngine;

//インベントリ上の武器の挙動を書くところ
//WeaponControllerとは違うので注意
public class Item_Weapon : Item
{
    public int Damage;
    public float FireRate;
    public float Speed;
    public float Range;
    public GameObject Prefab;
}
