using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    int number = 0;
    public TMP_Text NameText;
    public TMP_Text NumberText;
    public Image ItemImage;

    public void Init(int Number,string Name,Sprite image)
    {
        ItemImage.sprite = image;
        number = Number;
        NumberText.text = number.ToString();
        NameText.text = Name;
    }

    public void AddNumber(int number)
    {
        Debug.Log(this.number);
        this.number += number;
        NumberText.text = this.number.ToString();
    }
}
