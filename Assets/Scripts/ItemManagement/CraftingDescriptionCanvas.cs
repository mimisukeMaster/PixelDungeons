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

    public TMP_Text healthText;
    public TMP_Text moveSpeedText;

    public GameObject materialUI;
    public Color CraftableColor;
    public Transform materialUIParent;
    private GameObject[] addedElements;

    //武器
    public void Init(Item_Weapon item,CraftingElement.CraftingMaterial[] materials)
    {
        itemImage.sprite = item.ItemImage;
        itemNameText.text = item.ItemName;
        damageText.text = item.Damage.ToString();
        fireRateText.text = item.FireRate.ToString();
        speedText.text = item.Speed.ToString();
        rangeText.text = item.Range.ToString();

        addedElements = new GameObject[materials.Length];
        UpdateNumber(materials);
    }

    //アーマー
    public void Init(Item_Armor item,CraftingElement.CraftingMaterial[] materials)
    {
        itemImage.sprite = item.ItemImage;
        itemNameText.text = item.ItemName;
        healthText.text = item.MaxHP.ToString();
        moveSpeedText.text = item.playerSpeed.ToString();

        addedElements = new GameObject[materials.Length];
        UpdateNumber(materials);
    }

    public void UpdateNumber(CraftingElement.CraftingMaterial[] materials)
    {
        for(int i = 0;i < materialUIParent.childCount;i++)
        {
            Destroy(materialUIParent.GetChild(i).gameObject);
        }

        for(int i = 0;i < materials.Length;i++)
        {
            GameObject ui = Instantiate(materialUI,materialUIParent);
            
            //画像を設定
            ui.transform.GetChild(0).GetComponent<Image>().sprite = materials[i].Material.ItemImage;
            //数を設定
            ui.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{inventoryManager.GetItemNumber(materials[i].Material)}/{materials[i].number}";
            //素材が足りていたら緑にする
            if(inventoryManager.GetItemNumber(materials[i].Material) >= materials[i].number)ui.GetComponent<Image>().color = CraftableColor;
        }
    }
}
