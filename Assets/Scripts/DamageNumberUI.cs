using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class DamageNumberUI : MonoBehaviour
{
    private Vector3 target;

    [Tooltip("UIの上昇速度")]
    public float AscendSpeed;
    private float height;
    public TMP_Text text;

    public void Init(int damage,Vector3 target)
    {
        this.target = target;
        height = 0;
        text.text = damage.ToString();
    }

    void Update() 
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target);
        Debug.Log(screenPos);
        if(screenPos.z > 0)text.enabled = true;
        else text.enabled = false;
        transform.position = screenPos + Vector3.up * height;
        height += Time.deltaTime * AscendSpeed;
    }
}
