using UnityEngine;
using UnityEngine.UI;
using TMPro;

//UI上に表示されるもの
public class InventoryItem : MonoBehaviour
{
    public TMP_Text NameText;
    public Image ItemImage;
    public GameObject UsePanel;

    public TMP_Text NumberText;

    protected InventoryManager inventoryManager;

    protected Item item;

    public virtual void Init(Item item,InventoryManager inventoryManager,int number)
    {
        this.item = item;
        ItemImage.sprite = item.ItemImage;
        NameText.text = item.name;
        this.inventoryManager = inventoryManager;
        NumberText.text = number.ToString();
    }

    public void ChangeText(int number)
    {
        NumberText.text = number.ToString();
    }

    public void OnClick()//UIをクリックしたときの反応
    {
        UsePanel.SetActive(true);
    }

    public void OnUnhover()//マウスのホバーを外したとき
    {
        UsePanel.SetActive(false);
    }

    public void OnDropClick()//Dropをクリック
    {
        Debug.Log("Drop");
    }
}
