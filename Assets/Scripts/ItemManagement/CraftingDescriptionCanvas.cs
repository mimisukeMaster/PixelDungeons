using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class CraftingDescriptionCanvas : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Image itemImage;
    public TMP_Text itemNameText;
    public TMP_Text damageText;
    public TMP_Text fireRateText;
    public TMP_Text speedText;
    public TMP_Text rangeText;
    public GameObject materialUI;
    public Color CraftableColor;
    public Transform materialUIParent;
    private GameObject[] addedElements;

    public void Init(Item_Weapon item,CraftingElement.CraftingMaterial[] materials)
    {
        if(addedElements != null)
        {
            for(int i = 0;i<addedElements.Length;i++)
            {
                Destroy(addedElements[i]);
            }
        }

        itemImage.sprite = item.ItemImage;
        itemNameText.text = item.ItemName;
        damageText.text = item.Damage.ToString();
        fireRateText.text = item.FireRate.ToString();
        speedText.text = item.Speed.ToString();
        rangeText.text = item.Range.ToString();

        addedElements = new GameObject[materials.Length+1];

        for(int i = 0;i < materials.Length;i++)
        {
            GameObject ui = Instantiate(materialUI,materialUIParent);
            addedElements[i] = ui;
            //画像を設定
            ui.transform.GetChild(0).GetComponent<Image>().sprite = materials[i].Material.ItemImage;
            //数を設定
            ui.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{inventoryManager.GetItemNumber(materials[i].Material)}/{materials[i].number}";
            //素材が足りていたら緑にする
            if(inventoryManager.GetItemNumber(materials[i].Material) >= materials[i].number)ui.GetComponent<Image>().color = CraftableColor;
        }
    }
}
