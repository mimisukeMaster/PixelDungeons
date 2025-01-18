using UnityEditor.EditorTools;
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
    [Space(20)]
    public bool isHoming;
    [Space(20)]
    public bool isBomb;
    public float bombRadius;
    public int bombDamage;
    [Space(20)]
    public bool isRemain;
    [Tooltip("子のプレハブ")]
    public GameObject areaChild;
    [Tooltip("子の効果時間")]
    public float areaDuration;
    [Tooltip("子の攻撃間隔")]
    public float areaChildAttackInterval;
    [Tooltip("子のダメージ")]
    public int areaChildDamage;
}
