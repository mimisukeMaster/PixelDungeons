using UnityEditor.EditorTools;
using UnityEngine;

//武器の実際の処理を描くところ
//Item_Weaponとは違うので注意
public class WeaponController : MonoBehaviour
{
    [Tooltip("ダメージ")]
    public int Damage;
    [Tooltip("攻撃の間隔（秒）")]
    public float FireRate;
    [Tooltip("攻撃の速度")]
    public float speed;
    [Tooltip("射程")]
    public float Range;
    [Tooltip("実際の攻撃のオブジェクト")]
    public GameObject Attack;

    
}
