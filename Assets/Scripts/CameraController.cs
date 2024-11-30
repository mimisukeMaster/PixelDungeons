using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Weight = 10.0f;

    private float verticalRotation;
    private float horizontalRotation;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0f;
        horizontalRotation = 0f;
    }
    void Update()
    {
        // 入力受付
        float mouseX = Input.GetAxis("Mouse X") * Weight;
        float mouseY = Input.GetAxis("Mouse Y") * Weight;

        // 入力値整形
        verticalRotation -= mouseY;
        horizontalRotation += mouseX;

        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // 角度を反映、水平方向はプレイヤーごと回転
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.parent.transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);
    }
}