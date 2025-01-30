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
    public AudioClip BallFireLaunchSE;

    private bool isRightHand;
    private Animator animator;
    private float onClickTime;
    private bool isSuperAttack;
    private AudioSource audioSource;

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

    private void OnDestroy()
    {
        if(isRightHand)
        {
            PlayerController.controls.Player.RightAttack.started -= OnClickRight;
            PlayerController.controls.Player.RightAttack.canceled -= ReleaseClickRight;
        }
        else
        {
            PlayerController.controls.Player.LeftAttack.started -= OnClickLeft;
            PlayerController.controls.Player.LeftAttack.canceled -= ReleaseClickLeft;
        }
    }

    private void Start() 
    {
        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
    }

    // 右クリック押下
    private void OnClickRight(InputAction.CallbackContext context)
    {
        if (isRightHand && PlayerController.isGaming) Attack();
    }

    // 右クリック離す
    private void ReleaseClickRight(InputAction.CallbackContext context)
    {

    }

    // 左クリック押下
    private void OnClickLeft(InputAction.CallbackContext context) => onClickTime = Time.time;

    // 左クリック離す
    private void ReleaseClickLeft(InputAction.CallbackContext context)
    {
        if(!isRightHand && PlayerController.isGaming)
        {
            // 攻撃
            if (Time.time - onClickTime >= 0.5f) isSuperAttack = true;
            audioSource.PlayOneShot(BallFireLaunchSE);
            for(int i = 0;i < weapon.attackNumber;i++)Attack();
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        //攻撃する方向を決める
        Quaternion randomDirection = Quaternion.Euler(Random.Range(-weapon.attackDirectionSpread,weapon.attackDirectionSpread),
                                                        Random.Range(-weapon.attackDirectionSpread,weapon.attackDirectionSpread),
                                                        Random.Range(-weapon.attackDirectionSpread,weapon.attackDirectionSpread));

        GameObject attack = Instantiate(
            isSuperAttack ? weapon.SuperAttackPrefab : weapon.AttackPrefab, EmitTransform.position, EmitTransform.rotation);
        attack.GetComponent<Rigidbody>().linearVelocity = randomDirection * EmitTransform.forward * weapon.Speed;

        // 0チェック
        if (weapon.Speed != 0) attack.GetComponent<AttackController>().Init("Enemy", weapon.Range / weapon.Speed,weapon);
        else attack.GetComponent<AttackController>().Init("Enemy",0.5f, weapon);

        isSuperAttack = false;
        animator.SetTrigger("Attack");
    }
}
