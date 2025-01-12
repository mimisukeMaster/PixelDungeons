using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("ジャンプ力")]
    public float JumpForce = 5.0f;
    [Tooltip("移動の速さ")]
    public float MoveSpeed = 5.0f;
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

    private Rigidbody rb;
    public static ControlActions controls;
    private Vector2 moveInput;
    private bool isGrounded;
    public static bool isGaming;

    public static GameObject PlayerObject;

    private void Start() 
    {
        PlayerObject = gameObject;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

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

    private void FixedUpdate()
    {
        // 入力に合わせた移動
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y)
                             * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable() => controls.Disable();

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
        ClearCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
