using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Tooltip("アイテムの画像")]
    public Sprite itemImage;
    [Tooltip("アイテムのモデル")]
    public GameObject itemModel;
    public int damage;
    public float speed;

    public bool isDragged;
    private RectTransform rt;

    private void Start() 
    {
        rt = GetComponent<RectTransform>();
    }

    private void Update() 
    {
        //ドラッグされている間カーソルについていく
        if(isDragged)
        {
            Vector2 MousePos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                InventoryManager.rtParent_S,
                Input.mousePosition ,
                InventoryManager.CanvasRoot_S.worldCamera,
                out MousePos);
            rt.anchoredPosition = new Vector2(
                MousePos.x,
                MousePos.y);
        }
    }    
}
