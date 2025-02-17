using UnityEngine;

//武器のステータスを書くところ
//WeaponControllerとは違うので注意
public class Item_Weapon : Item
{
    public int Damage;
    public float FireRate;
    public float Speed;
    public float Range;
    public int Penetration;
    public int attackNumber = 1;
    public float attackDirectionSpread = 0;
    public GameObject WeaponModelPrefab;
    public GameObject AttackPrefab;
    public GameObject SuperAttackPrefab;
    [Space(20)]
    public bool IsHoming;
    [Tooltip("誘導の強さ 度/s")]
    public float HomingAngle;
    [Tooltip("誘導が認識する距離")]
    public float DetectionDistance;
    [Space(20)]
    public bool isBeam;
    public LineRenderer beamLineRenderer;
    public float beamChargeTime;
    public float beamEmissionTime;
    public float beamLength;
    [Space(20)]
    public bool IsBomb;
    public float BombRadius;
    public int BombDamage;
    [Space(20)]
    public bool IsRemain;
    [Tooltip("子のプレハブ")]
    public GameObject AreaChild;
    [Tooltip("子の効果時間")]
    public float AreaDuration;
    [Tooltip("子の攻撃間隔")]
    public float AreaChildAttackInterval;
    [Tooltip("子のダメージ")]
    public int AreaChildDamage;
}
