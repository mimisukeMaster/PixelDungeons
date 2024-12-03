using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [Tooltip("攻撃のプレハブ")]
    public GameObject Attack;
    public float AttackSpeed;
    private float attackInterval;
    public float Damage;
    public float ProjectileSpeed;
    public int TargetLayer;
    

    private void Update() 
    {
        attackInterval += Time.deltaTime;
        //発射する
        if(Input.GetKey(KeyCode.Space) && attackInterval >= AttackSpeed)
        {
            GameObject attack = Instantiate(Attack,transform.position,Quaternion.identity);
            attack.GetComponent<AttackController>().Init(TargetLayer,Damage,ProjectileSpeed);
            attackInterval = 0;
        }    
    }

}
