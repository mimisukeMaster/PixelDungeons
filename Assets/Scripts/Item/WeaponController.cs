using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

//武器の実際の処理を描くところ
//プレハブに付ける
//Item_Weaponとは違うので注意
public class WeaponController : MonoBehaviour
{
    private int damage;
    private float fireRate;
    private float speed;
    private float range;
    private GameObject AttackPrefab;

    public Transform EmmitTransform;

    private bool isRightHand;

    private bool continueAttack;
    private Animator animator;

    public void Init(int damage,float fireRate,float range,float speed,bool isRightHand,GameObject attackPrefab,Transform attackTransform)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        this.range = range;
        this.speed = speed;
        this.isRightHand = isRightHand;
        animator = transform.parent.gameObject.GetComponent<Animator>();
        animator.SetFloat("Speed",1/fireRate);
        AttackPrefab = attackPrefab;
        EmmitTransform = attackTransform;

        if(isRightHand)
        {
            PlayerController.controls.Player.RightAttack.started += OnClickRight;
            PlayerController.controls.Player.RightAttack.canceled += ReleaseClickRight;
        }
        else
        {
            PlayerController.controls.Player.LeftAttack.started += OnClickLeft;
            PlayerController.controls.Player.LeftAttack.canceled += ReleaseClickLeft;
        }
    }

    private float interval = 0.0f;

    private void Update() 
    {
        //攻撃
        interval += Time.deltaTime;
        if(interval >= fireRate && continueAttack)
        {   
            Attack();
            interval = 0;
        }
    }

    private void OnClickRight(InputAction.CallbackContext context)//右クリック押す
    {
        if(isRightHand)continueAttack = true;
    }

    private void ReleaseClickRight(InputAction.CallbackContext context)//右クリック離す
    {
        if(isRightHand)continueAttack = false;
    }

    private void OnClickLeft(InputAction.CallbackContext context)//左クリック押す
    {
        if(!isRightHand)continueAttack = true;
    }

    private void ReleaseClickLeft(InputAction.CallbackContext context)//左クリック離す
    {
        if(!isRightHand)continueAttack = false;
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        GameObject attack = Instantiate(AttackPrefab,EmmitTransform.position,EmmitTransform.rotation);
        attack.GetComponent<Rigidbody>().linearVelocity = EmmitTransform.forward * speed;
        if(speed != 0)attack.GetComponent<AttackController>().Init("Enemy",damage,range / speed);
        //0チェック
        else attack.GetComponent<AttackController>().Init("Enemy",damage,0.5f);

        animator.SetTrigger("Attack");
    }
}
