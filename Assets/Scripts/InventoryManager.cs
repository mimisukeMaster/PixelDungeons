using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.EditorTools;

public class InventoryManager : MonoBehaviour
{
    [Tooltip("バッグのスロットの親")]
    public Transform BagParent;
    public static Dictionary<int,InventorySlot> inventorySlots_S = new Dictionary<int, InventorySlot>();
    [Tooltip("右手のスロット")]
    public InventorySlot RightSlot;
    [Tooltip("左手のスロット")]
    public InventorySlot LeftSlot;
    public static int InventorySlotInHover_S = 100;//100が何もない状態　インベントリスロット数が100以上になるときは要相談
    public static Item ItemBeingDragged_S;
    [Tooltip("マウスの一番親のキャンバス")]
    public Canvas CanvasRoot;
    public static Canvas CanvasRoot_S;
    [Tooltip("アイテムの画像の親")]
    public RectTransform rtParent;
    public static RectTransform rtParent_S; 

    void Start()
    {
        CanvasRoot_S = CanvasRoot;
        rtParent_S = rtParent;
        //各スロットをリストに追加
        for(int i = 0;i < BagParent.childCount;i++)
        {
            inventorySlots_S.Add(i,BagParent.GetChild(i).GetComponent<InventorySlot>());
        }
        inventorySlots_S.Add(inventorySlots_S.Count,LeftSlot);//左手追加
        inventorySlots_S.Add(inventorySlots_S.Count,RightSlot);//右手追加
    }

    //アイテムを移動する
    public bool Inventory(int id)
    {
        if(inventorySlots_S[id] != null)
        {
            return false;
        }
        inventorySlots_S[id].item = ItemBeingDragged_S;
        return true;
    }
}
