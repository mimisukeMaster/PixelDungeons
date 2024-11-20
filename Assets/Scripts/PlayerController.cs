using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float JumpForce = 5.0f;
    public float moveSpeed = 5.0f;
    
    private Rigidbody rb;
    private ControlActions controls;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // インスタンス生成
        controls = new ControlActions();

        // InputSystemでの入力に対応するリスナーを追加
        controls.Player.Jump.performed += OnJumpPerformed;

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
    }

    private void FixedUpdate()
    {
        // 入力に合わせた移動
        Vector3 move = new Vector3(moveInput.x , 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

    }

    private void OnEnable()
    {
        // Input Actionsを有効化
        controls.Enable();
    }

    private void OnDisable()
    {
        // Input Actionsを無効化
        controls.Disable();
    }

    private void OnJumpPerformed(InputAction.CallbackContext controls) {
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void OnMovePerformed(InputAction.CallbackContext context) {
        // 入力されたアクションの値を読み取る
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) {
        // 移動入力が無くなったら止まる
        moveInput = Vector2.zero;
    }
}
