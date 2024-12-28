using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

//武器の実際の処理を描くところ
//プレハブに付ける
//Item_Weaponとは違うので注意
public class WeaponController : MonoBehaviour
{
    [System.NonSerialized]
    public Item_Weapon weapon;

    public Transform EmitTransform;

    private bool isRightHand;

    private bool continueAttack;
    private Animator animator;

    public void Init(Item_Weapon weapon,bool isRightHand,Transform attackTransform)
    {
        this.weapon = weapon;
        this.isRightHand = isRightHand;
        animator = transform.parent.gameObject.GetComponent<Animator>();
        animator.SetFloat("Speed",1/weapon.FireRate);
        EmitTransform = attackTransform;

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
        if(interval >= weapon.FireRate && continueAttack)
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
        GameObject attack = Instantiate(weapon.AttackPrefab, EmitTransform.position, EmitTransform.rotation);
        attack.GetComponent<Rigidbody>().linearVelocity = EmitTransform.forward * weapon.Speed;
        if (weapon.Speed != 0) attack.GetComponent<AttackController>().Init("Enemy", weapon.Damage, weapon.Range / weapon.Speed);
        //0チェック
        else attack.GetComponent<AttackController>().Init("Enemy", weapon.Damage, 0.5f);

        animator.SetTrigger("Attack");
    }
}
