using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//武器の実際の処理を描くところ
//プレハブに付ける
//Item_Weaponとは違うので注意
public class WeaponController : MonoBehaviour
{
    [System.NonSerialized]
    public Item_Weapon weapon;
    public Transform EmitTransform;
    public AudioClip[] AttackSEs;

    private bool isClicking;
    private float nextAttackTime = 0;
    private bool isRightHand;
    private Animator animator;
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
        SceneManager.sceneLoaded += ResetClick;
    }

    //DontDestroyOnLoadのため
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

    private void ResetClick(Scene scene,LoadSceneMode mode)
    {
        isClicking = false;
    }

    private void Start() 
    {
        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();
    }

    // 右クリック押下
    private void OnClickRight(InputAction.CallbackContext context)
    {
        if (isRightHand && PlayerController.isGaming) isClicking = true;
    }
    
    // 右クリック離す
    private void ReleaseClickRight(InputAction.CallbackContext context)
    {
        if(isRightHand)isClicking = false;
    }

    // 左クリック押下
    private void OnClickLeft(InputAction.CallbackContext context)
    {
        if(!isRightHand && PlayerController.isGaming)isClicking = true;
    }

    // 左クリック離す
    private void ReleaseClickLeft(InputAction.CallbackContext context)
    {
        if(!isRightHand)isClicking = false;
    }

    void Update()
    {
        if(isClicking && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + weapon.FireRate;
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        // ランダムに効果音を鳴らす
        audioSource.PlayOneShot(AttackSEs[Random.Range(0, 3)]);

        //攻撃する方向を決める
        Quaternion randomDirection = Quaternion.Euler(Random.Range(-weapon.attackDirectionSpread,weapon.attackDirectionSpread),
                                                        Random.Range(-weapon.attackDirectionSpread,weapon.attackDirectionSpread),
                                                        Random.Range(-weapon.attackDirectionSpread,weapon.attackDirectionSpread));

        GameObject attack = Instantiate(
            weapon.AttackPrefab, EmitTransform.position, EmitTransform.rotation);
        attack.GetComponent<Rigidbody>().linearVelocity = randomDirection * EmitTransform.forward * weapon.Speed;

        // 0チェック
        if (weapon.Speed != 0) attack.GetComponent<AttackController>().Init("Enemy", weapon.Range / weapon.Speed,weapon);
        else attack.GetComponent<AttackController>().Init("Enemy",0.5f, weapon);

        animator.SetTrigger("Attack"+Random.Range(1,3));
    }
}
