using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [Tooltip("攻撃のプレハブ")]
    public GameObject Attack;
    public float AttackSpeed;
    private float attackInterval;
    public int Damage;
    public float ProjectileSpeed;
    public string TargetTag;
    

    private void Update() 
    {
        attackInterval += Time.deltaTime;

        //発射する
        if(Input.GetKey(KeyCode.Space) && attackInterval >= AttackSpeed)
        {
            GameObject attack = Instantiate(Attack,transform.position,Quaternion.identity);
            attack.GetComponent<AttackController>().Init(TargetTag, Damage);
            attackInterval = 0;
        }    
    }

}
