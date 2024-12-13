using UnityEditor.EditorTools;
using UnityEngine;

public class HPController : MonoBehaviour
{   
    [Tooltip("現在の体力")]
    public int HP;
    [Tooltip("体力の最大値")]
    public int MaxHP = 100;
    [Tooltip("TagがEnemyでない場合は必要なし")]
    public EnemyController EnemyController;


    void Start()
    {
        HP = MaxHP;
    }

    void Update()
    {
        
    }

    public void Damaged(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            if(gameObject.CompareTag("Player"))
            {

            }
            else if(gameObject.CompareTag("Enemy")) EnemyController.OnDied();
        }
    }

    public void Healed(int heal)
    {
        HP += heal;
        if(HP > MaxHP) HP = MaxHP;
    }
}
