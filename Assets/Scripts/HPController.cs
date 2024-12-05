using UnityEngine;

public class HPController : MonoBehaviour
{   
    public int HP;
    public int MaxHP = 100;
    public EnemyController Controller;


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
        if(HP < 0)
        {
            if(gameObject.CompareTag("Player"))
            {
                

            }
            else if(gameObject.CompareTag("Enemy"))
            {
                Controller.OnDied();

            }
        }
    }

    public void Healed(int heal)
    {
        HP += heal;
        if(HP > MaxHP) HP = MaxHP;
    }
}
