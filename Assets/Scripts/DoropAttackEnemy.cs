using UnityEngine;

public class DoropAttackEnemy : EnemyController 
{
    
    public float Altitude =10.0f;

    
    void Start()
    {   transform.position = new Vector3(transform.position.x,Altitude,transform.position.z);
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        //検知範囲を上書きする、高度の約sqrt(2/√3)倍
        DetectionRadius = Altitude * 1.15f;
        

    }


    void Update()
    {
        
    }
}
