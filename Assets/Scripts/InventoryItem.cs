using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public TMP_Text NameText;
    public Image ItemImage;
    public GameObject UsePanel;

    public virtual void Init(string Name,Sprite image)
    {
        ItemImage.sprite = image;
        NameText.text = Name;
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
