using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int Id;
    public Item Item = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("InventoryItem"))
        {
            InventoryManager.InventorySlotInHover_S = Id;
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
        if(Item != null)
        {
            InventoryManager.ItemBeingDragged_S = Item;
            Item.isDragged = true;
        }
    }

    public void OnDragEnd()
    {
        Item.isDragged = false;
        InventoryManager.ItemBeingDragged_S = null;
        if(InventoryManager.InventorySlotInHover_S == 100)//アイテムがスロットから外れていたら元あった場所に戻して終わる
        {
            Item.gameObject.GetComponent<RectTransform>().position = transform.position;
            return;
        }
        InventorySlot destination = InventoryManager.inventorySlots_S[InventoryManager.InventorySlotInHover_S];
        if(destination.Item == null)
        {
            destination.Item = Item;
            Item.gameObject.GetComponent<RectTransform>().position = destination.gameObject.transform.position;
            Item = null;
        }
        else
        {
            Item.gameObject.GetComponent<RectTransform>().position = transform.position;
        }
    }
}
