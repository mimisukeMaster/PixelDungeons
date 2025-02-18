using UnityEngine;
using TMPro;
using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine.UI;

//CraftCanvasのCanvas下のContentに付ける
//Craftingがこれに依存している
public class CraftingStation : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingDescriptionCanvas descriptionCanvasWeapon;
    public CraftingDescriptionCanvas descriptionCanvasArmor;
    public Scrollbar slider;

    CraftingElement.CraftingMaterial[] materials;
    Item resultItem;
    int resultNumber;

    public void ShowDescription(CraftingElement.CraftingMaterial[] materials,Item resultItem,int resultNumber)
    {
        if(resultItem is Item_Weapon)
        {
            descriptionCanvasArmor.gameObject.SetActive(false);
            descriptionCanvasWeapon.gameObject.SetActive(true);
            descriptionCanvasWeapon.Init((Item_Weapon)resultItem,materials);
        }
        else if(resultItem is Item_Armor)
        {
            descriptionCanvasWeapon.gameObject.SetActive(false);
            descriptionCanvasArmor.gameObject.SetActive(true);
            descriptionCanvasArmor.Init((Item_Armor)resultItem,materials);
        }
        this.materials = materials;
        this.resultItem = resultItem;
        this.resultNumber = resultNumber;
    }

    void OnDisable()
    {
        descriptionCanvasArmor.gameObject.SetActive(false);
        descriptionCanvasWeapon.gameObject.SetActive(false);
        slider.value = 1;
    }

    public bool Craft()
    {
        Item[] material = new Item[materials.Length ];
        int[] numbers = new int[materials.Length];

        for(int i = 0;i < materials.Length; i ++)
        {
            material[i] = materials[i].Material;
            numbers[i] = materials[i].number;
        }
        bool isCraftable = inventoryManager.Craft(material,numbers,resultItem,resultNumber);

        if(resultItem is Item_Weapon)descriptionCanvasWeapon.UpdateNumber(materials);
        else descriptionCanvasArmor.UpdateNumber(materials);
        return isCraftable;
    }
}
