using UnityEngine;

public class HPController : MonoBehaviour
{   
    public int HP;
    public int MaxHP = 100;



    
    
    void Start()
    {
        HP = MaxHP ;
    }

    void Update()
    {
        
    }

    void Damaged(int damage){
        HP = HP - damage;
        if(HP < 0){
            //ゲームオーバー処理を呼び出す
        }



    }
    void Healed(int heal){
        HP = HP + heal;
        if(HP > MaxHP) HP = MaxHP;


    }

   

}