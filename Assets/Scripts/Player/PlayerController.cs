using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Tooltip("ジャンプ力")]
    public float JumpForce = 5.0f;
    [Tooltip("移動の速さ")]
    public float MoveSpeed = 5.0f;
    [HideInInspector]
    public Vector3 lastMoveDirection;
    [Tooltip("近距離攻撃検知範囲")]
    public float NearAttackRange = 1.0f;
    [Header("UI")]
    [Tooltip("インベントリのキャンバス")]
    public GameObject InventoryCanvas;
    [Tooltip("ゲームオーバーのキャンバス")]
    public GameObject GameOverCanvas;
    [Tooltip("ゲーム中のキャンバス")]
    public GameObject GamingCanvas;
    [Tooltip("クリア時のキャンバス")]
    public GameObject ClearCanvas;
    [Tooltip("ゴールゲート通過SE")]
    public AudioClip GoalGateSE;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private AudioSource audioSource;

    public static ControlActions controls;
    public static bool isGaming;
    public static GameObject PlayerObject;

    public static PlayerController singleton;

    private void Start() 
    {
        PlayerObject = gameObject;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        //Coreをシングルトン化
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        DontDestroyOnLoad(transform.parent.gameObject);

        rb = GetComponent<Rigidbody>();
        audioSource = transform.parent.GetComponentInChildren<AudioSource>();

        InventoryCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        GamingCanvas.SetActive(true);

        // インスタンス生成
        controls = new ControlActions();

        // InputSystemでの入力に対応するリスナーを追加
        controls.Player.Jump.performed += OnJumpPerformed;
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.OpenInventory.performed += SwitchInventory;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        rb.isKinematic = true;
        rb.isKinematic = false;
        transform.position = new Vector3(0f, 0.5f, 0f);
        transform.rotation = Quaternion.identity;
        moveInput = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;

        if (ClearCanvas.activeSelf) ClearCanvas.SetActive(false);
    }

    private void FixedUpdate()
    {
        // 入力に合わせた移動
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y)
                             * MoveSpeed * Time.fixedDeltaTime;
        lastMoveDirection = move * MoveSpeed;
        rb.MovePosition(rb.position + move);
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable()
    {
        if(singleton != this)return;
        controls.Disable();
    }

    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // アイテム
        if (collision.gameObject.CompareTag("DropItem"))
        {
            DropItem item = collision.gameObject.GetComponent<DropItem>();
            GetComponent<InventoryManager>().AddItem(item.Item, item.Number);
            Destroy(collision.gameObject);
        }
        // 着地
        isGrounded = true;

        // ゴールゲート
        if (collision.gameObject.CompareTag("GoalGate")) OnCleared();
    }

    private void OnJumpPerformed(InputAction.CallbackContext controls)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isGaming) moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) => moveInput = Vector2.zero;

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

    public void OnCleared()
    {
        audioSource.PlayOneShot(GoalGateSE);
        ClearCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
