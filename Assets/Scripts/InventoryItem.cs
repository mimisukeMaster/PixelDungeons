using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text NumberText;
    public Image ItemImage;
    public GameObject UsePanel;
    public int number;

    public void Init(int Number,string Name,Sprite image)
    {
        ItemImage.sprite = image;
        number = Number;
        NumberText.text = number.ToString();
        NameText.text = Name;
    }

    public void AddNumber(int number)
    {

    }

    public void OnClick()
    {
        UsePanel.SetActive(true);
    }

    public void OnUnhover()
    {
        UsePanel.SetActive(false);
    }
}
