using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int Id;
    public Item item = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("InventoryItem"))
        {
            InventoryManager.InventorySlotInHover_S = Id;
            Debug.Log(InventoryManager.InventorySlotInHover_S);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("InventoryItem")&&InventoryManager.InventorySlotInHover_S == Id)
        {
            InventoryManager.InventorySlotInHover_S = 100;
        }
    }

    public void OnHover()
    {
        
    }

    public void OnUnhover()
    {

    }

    public void OnDragStart()
    {
        if(item != null)
        {
            InventoryManager.ItemBeingDragged_S = item;
            item.isDragged = true;
        }
    }

    public void OnDragEnd()
    {
        item.isDragged = false;
        InventoryManager.ItemBeingDragged_S = null;
        if(InventoryManager.InventorySlotInHover_S == 100)//アイテムがスロットから外れていたら元あった場所に戻して終わる
        {
            item.gameObject.GetComponent<RectTransform>().position = transform.position;
            return;
        }
        InventorySlot destination = InventoryManager.inventorySlots_S[InventoryManager.InventorySlotInHover_S];
        if(destination.item == null)
        {
            destination.item = item;
            item.gameObject.GetComponent<RectTransform>().position = destination.gameObject.transform.position;
            item = null;
        }
        else
        {
            item.gameObject.GetComponent<RectTransform>().position = transform.position;
        }
    }
}
