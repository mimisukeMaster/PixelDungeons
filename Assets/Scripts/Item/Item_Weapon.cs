using UnityEngine;

//武器のステータスを書くところ
//WeaponControllerとは違うので注意
public class Item_Weapon : Item
{
    public int Damage;
    public float FireRate;
    public float Speed;
    public float Range;
    public GameObject Prefab;
    public GameObject AttackPrefab;
    public GameObject SuperAttackPrefab;

}
