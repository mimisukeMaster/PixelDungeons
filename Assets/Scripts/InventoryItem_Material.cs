using UnityEngine;
using TMPro;

public class InventoryItem_Material : InventoryItem
{
    public int Number;
    public TMP_Text NumberText;

    public void Init(string name, Sprite image,int number)
    {
        base.Init(name, image);
        Number = number;
        NumberText.text = Number.ToString();
    }

    public void AddNumber(int number)
    {
        Number += number;
        NumberText.text = Number.ToString();
    }
}
