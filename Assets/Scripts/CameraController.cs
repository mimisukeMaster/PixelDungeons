using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Weight = 10.0f;
    Camera camera;
    void Start()
    {
        camera = gameObject .GetComponent <Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Weight;
        float mouseY = Input.GetAxis("Mouse Y") * Weight;
        
        mouseY = Mathf.Clamp(mouseY,-90f,90f) * -1;
        transform.localRotation = Quaternion.Euler(mouseY,0,0);
        transform.parent.transform.Rotate(Vector3.up * mouseX);





    }

    
}