using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("ジャンプ力")]
    public float JumpForce = 5.0f;
    [Tooltip("移動の速さ")]
    public float MoveSpeed = 5.0f;
    [Tooltip("カメラ")]
    public Camera PlayerCam;

    [Header("遠距離攻撃")]
    [Tooltip("オブジェクト参照")]
    public GameObject WeaponSlotL;
    [Tooltip("魔法オブジェクト参照")]
    public GameObject MagicObj;
    [Tooltip("強い魔法オブジェクト参照")]
    public GameObject SuperMagicObj;
    [Tooltip("遠距離攻撃力")]
    public int FarAttack = 10;
    [Tooltip("強い遠距離攻撃")]
    public int SuperFarAttack = 20;
    [Tooltip("遠距離攻撃発射位置")]
    public Transform MagicPos;
    [Tooltip("遠距離攻撃発射の速さ")]
    public float AttackSpeed = 5.0f;

    [Header("近距離攻撃")]
    [Tooltip("オブジェクト参照")]
    public GameObject WeaponSlotR;
    [Tooltip("強い攻撃のパーティクル")]
    public ParticleSystem SuperNearAttackParticle;
    [Tooltip("近距離攻撃力")]
    public int NearAttack = 20;
    [Tooltip("強い近距離攻撃")]
    public int SuperNearAttack = 30;
    [Tooltip("近距離攻撃検知範囲")]
    public float NearAttackRange = 1.0f;
    [Header("UI")]
    [Tooltip("インベントリのキャンバス")]
    public GameObject InventoryCanvas;
    [Tooltip("ゲームオーバーのキャンバス")]
    public GameObject GameOverCanvas;
    [Tooltip("ゲーム中のキャンバス")]
    public GameObject GamingCanvas;

    private Rigidbody rb;
    private ControlActions controls;
    private Vector2 moveInput;
    private bool isGrounded;
    public static bool isGaming;
    private float clickedTime;
    private Animator animatorL;
    private Animator animatorR;
    private AttackController attackController;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animatorL = WeaponSlotL.GetComponent<Animator>();
        animatorR = WeaponSlotR.GetComponent<Animator>();
        attackController = WeaponSlotR.GetComponentInChildren<AttackController>();
        isGaming = true;

        InventoryCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        GamingCanvas.SetActive(true);

        // インスタンス生成
        controls = new ControlActions();

        // InputSystemでの入力に対応するリスナーを追加
        controls.Player.Jump.performed += OnJumpPerformed;
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Attack.started += OnTimerStart;
        controls.Player.Attack.canceled += OnTimerStop;
        controls.Player.OpenInventory.performed += SwitchInventory;
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

    private void OnCollisionEnter(Collision collision)
    {
        //アイテム関係
        if (collision.gameObject.CompareTag("DropItem"))
        {
            DropItem item = collision.gameObject.GetComponent<DropItem>();
            GetComponent<InventoryManager>().AddItem(item.Item,item.Number);
            Destroy(collision.gameObject);
        }
        //着地
        isGrounded = true;
    }

    private void OnJumpPerformed(InputAction.CallbackContext controls)
    {
        if (isGrounded)
        {
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
        if (!isGaming) return;

        // 遠距離攻撃
        if (context.control == Mouse.current.leftButton)
        {
            animatorL.SetTrigger("Attack");
            GameObject magic = Instantiate(MagicObj, MagicPos.position, Quaternion.identity);
            magic.GetComponent<Rigidbody>().linearVelocity = PlayerCam.transform.forward * AttackSpeed;
            magic.GetComponent<AttackController>().Init("Enemy", FarAttack);

            Destroy(magic, 5.0f);
        }
        // 近距離攻撃 内部処理はWeaponEventHandler.csから呼びだす
        else if (context.control == Mouse.current.rightButton)
        {
            animatorR.SetTrigger("Attack");

            attackController.Init("Enemy", NearAttack);
        }
    }

    /// <summary>
    /// 溜め操作時のプレイヤー攻撃処理
    /// </summary>
    /// <remarks>強めの攻撃</remarks>
    /// <param name="context"></param>
    private void OnSuperAttack(InputAction.CallbackContext context)
    {
        if (!isGaming) return;

        // 強い遠距離攻撃 弾を変える
        if (context.control == Mouse.current.leftButton)
        {
            animatorL.SetTrigger("SuperAttack");

            GameObject superMagic = Instantiate(SuperMagicObj, MagicPos.position, Quaternion.identity);
            superMagic.GetComponent<Rigidbody>().linearVelocity = PlayerCam.transform.forward * AttackSpeed;
            superMagic.GetComponent<AttackController>().Init("Enemy", SuperFarAttack);

            Destroy(superMagic, 5.0f);

        }
        // 強い近距離攻撃 パーティクルを出す 内部処理はWeaponEventHandler.csから呼びだす
        else if (context.control == Mouse.current.rightButton)
        {
            animatorR.SetTrigger("SuperAttack");

            attackController.Init("Enemy", SuperNearAttack);

            if (SuperNearAttackParticle) Instantiate(SuperNearAttackParticle, WeaponSlotR.transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// インベントリを開く/閉じる
    /// </summary>
    /// <param name="context"></param>
    private void SwitchInventory(InputAction.CallbackContext context)
    {
        InventoryCanvas.SetActive(!InventoryCanvas.activeSelf);
        GamingCanvas.SetActive(!GamingCanvas.activeSelf);

        //インベントリが開いたとき
        if (InventoryCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            isGaming = false;
            Time.timeScale = 0;
        }
        //インベントリが閉じたとき
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            isGaming = true;
            Time.timeScale = 1;
        }
    }

    public void OnDied()
    {
        InventoryCanvas.SetActive(false);
        GamingCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        isGaming = false;
        Time.timeScale = 0;
    }
}
