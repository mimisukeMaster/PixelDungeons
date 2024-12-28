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
    private float onClickTime;
    private bool isSuperAttack;

    public void Init(Item_Weapon weapon,bool isRightHand,Transform attackTransform)
    {
        this.weapon = weapon;
        this.isRightHand = isRightHand;
        animator = transform.parent.gameObject.GetComponent<Animator>();
        animator.SetFloat("Speed", 1 / weapon.FireRate);
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

    private void Update() 
    {

    }

    // 右クリック押下
    private void OnClickRight(InputAction.CallbackContext context)
    {
        if (isRightHand) Attack();
    }

    // 右クリックが離す
    private void ReleaseClickRight(InputAction.CallbackContext context)
    {

    }

    // 左クリック押下
    private void OnClickLeft(InputAction.CallbackContext context) => onClickTime = Time.time;

    //左クリック離す
    private void ReleaseClickLeft(InputAction.CallbackContext context)
    {
        if(!isRightHand)
        {
            // 攻撃
            if (Time.time - onClickTime >= 0.5f) isSuperAttack = true;
            Attack();
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        GameObject attack = Instantiate(
            isSuperAttack ? weapon.SuperAttackPrefab : weapon.AttackPrefab, EmitTransform.position, EmitTransform.rotation);
        attack.GetComponent<Rigidbody>().linearVelocity = EmitTransform.forward * weapon.Speed;
        if (weapon.Speed != 0) attack.GetComponent<AttackController>().Init("Enemy", isSuperAttack ? weapon.Damage * 2 : weapon.Damage, weapon.Range / weapon.Speed);
        //0チェック
        else attack.GetComponent<AttackController>().Init("Enemy", weapon.Damage, 0.5f);

        isSuperAttack = false;
        animator.SetTrigger("Attack");
    }
}
