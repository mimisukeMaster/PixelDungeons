using UnityEngine;

public class BillBoardUI : MonoBehaviour
{
    private Transform myTransform;
    private void Start() 
    {
        myTransform = transform;
    }

    /// <summary>
    /// UIを常にカメラの方向へ向かせる
    /// </summary>
    private void Update() 
    {
        myTransform.rotation = Camera.main.transform.rotation;
    }
}
