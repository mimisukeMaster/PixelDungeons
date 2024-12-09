using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    [Tooltip("ジャンプ力")]
    public float JumpForce = 5.0f;
    [Tooltip("移動の速さ")]
    public float MoveSpeed = 5.0f;
    [Tooltip("遠距離攻撃の速さ")]
    public float AttackSpeed = 5.0f;
    [Tooltip("遠距離攻撃力")]
    public int Attack = 10;
    [Tooltip("カメラ")]
    public Camera PlayerCam;
    [Tooltip("遠距離攻撃用")]
    public GameObject MagicObj;
    [Tooltip("遠距離攻撃発射位置")]
    public Transform MagicPos;
    [Tooltip("近距離攻撃用")]
    public Animator WeaponAnim;
    
    private Rigidbody rb;
    private ControlActions controls;
    private Vector2 moveInput;
    private bool isGrounded;
    private float clickedTime;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // インスタンス生成
        controls = new ControlActions();

        // InputSystemでの入力に対応するリスナーを追加
        controls.Player.Jump.performed += OnJumpPerformed;
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Attack.started += OnTimerStart;
        controls.Player.Attack.canceled += OnTimerStop;
    }

    private void FixedUpdate()
    {
        // 入力に合わせた移動
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y)
                             * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable() => controls.Disable();

    private void OnCollisionEnter(Collision collision) => isGrounded = true;

    private void OnJumpPerformed(InputAction.CallbackContext controls)
    {
        if (isGrounded) {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

    private void OnMoveCanceled(InputAction.CallbackContext context) => moveInput = Vector2.zero;
    
    private void OnTimerStart(InputAction.CallbackContext context) => clickedTime = Time.time;
    
    private void OnTimerStop(InputAction.CallbackContext context)
    {
        if (Time.time - clickedTime < 0.4f) OnAttack(context);
        else OnSuperAttack(context);
    }

    /// <summary>
    /// 通常操作のプレイヤー攻撃処理
    /// </summary>
    /// <param name="context"></param>
    private void OnAttack(InputAction.CallbackContext context)
    {
        // 遠距離攻撃
        if (context.control == Mouse.current.rightButton)
        {
            GameObject magic = Instantiate(MagicObj, MagicPos.position, Quaternion.identity); 
                    magic.GetComponent<Rigidbody>().linearVelocity = PlayerCam.transform.forward * AttackSpeed;
            magic.GetComponent<AttackController>().Init("Enemy", Attack);

            Destroy(magic, 5.0f);
        }
        // 近距離攻撃
        else if (context.control == Mouse.current.leftButton)
        {
            WeaponAnim.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// 溜め操作時のプレイヤー攻撃処理
    /// </summary>
    /// <remarks>強めの攻撃</remarks>
    /// <param name="context"></param>
    private void OnSuperAttack(InputAction.CallbackContext context)
    {
        if (context.control == Mouse.current.rightButton)
        {

        }
        else if (context.control == Mouse.current.leftButton)
        {
            WeaponAnim.SetTrigger("SuperAttack");
        }
    }
}
