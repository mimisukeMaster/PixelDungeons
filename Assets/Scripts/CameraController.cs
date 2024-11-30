using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Weight = 10.0f;
    private float xRotation;

    Camera camera;
    void Start()
    {
        camera = gameObject .GetComponent <Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Weight;
        float mouseY = Input.GetAxis("Mouse Y") * Weight;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);
        transform.localRotation = Quaternion.Euler(xRotation,0,0);
        transform.parent.transform.Rotate(Vector3.up * mouseX);





    }

    
}