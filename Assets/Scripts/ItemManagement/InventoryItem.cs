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

    [System.NonSerialized]
    public int number;

    protected InventoryManager inventoryManager;

    protected Item item;

    public virtual void Init(Item item,InventoryManager inventoryManager,int number)
    {
        this.item = item;
        ItemImage.sprite = item.ItemImage;
        NameText.text = item.name;
        this.inventoryManager = inventoryManager;
        this.number = number;
        NumberText.text = this.number.ToString();
    }

    public void AddNumber(int number)
    {
        this.number += number;
        NumberText.text = this.number.ToString();
    }

    public bool GetRemovable(int number)
    {
        if(this.number >= number)return true;
        else return false;
    }

    public void SubNumber(int number)
    {
        this.number -= number;
        NumberText.text = this.number.ToString();
        if(this.number <= 0)
        {
            inventoryManager.RemoveItem(item);
            Destroy(gameObject);
        }
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
