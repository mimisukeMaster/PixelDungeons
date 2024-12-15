using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Weight = 10.0f;
    public float ScopeFOV = 35.0f;

    private float verticalRotation;
    private float horizontalRotation;
    private Camera playerCam;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0f;
        horizontalRotation = 0f;

        playerCam = GetComponent<Camera>();
    }
    void Update()
    {
        // 入力受付
        float mouseX = Input.GetAxis("Mouse X") * Weight;
        float mouseY = Input.GetAxis("Mouse Y") * Weight;
        bool isLeftShift = Input.GetKey(KeyCode.LeftShift);

        // 入力値整形
        verticalRotation -= mouseY;
        horizontalRotation += mouseX;

        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // 角度を反映、水平方向はプレイヤーごと回転
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.parent.transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);

        // スコープモード 線形補間を適用
        if (isLeftShift)
        {
            if (playerCam.fieldOfView - ScopeFOV > 0.1f) playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ScopeFOV, 0.5f);
            else playerCam.fieldOfView = ScopeFOV;
        }
        else playerCam.fieldOfView = 60;
    }
}