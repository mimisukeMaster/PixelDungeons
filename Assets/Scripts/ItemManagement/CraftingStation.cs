using UnityEngine;
using TMPro;
using NUnit.Framework.Interfaces;

//CraftCanvasのCanvas下のContentに付ける
//Craftingがこれに依存している
public class CraftingStation : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingDescriptionCanvas descriptionCanvas;

    CraftingElement.CraftingMaterial[] materials;
    Item resultItem;
    int resultNumber;

    public void ShowDescription(CraftingElement.CraftingMaterial[] materials,Item_Weapon resultItem,int resultNumber)
    {
        descriptionCanvas.gameObject.SetActive(true);
        descriptionCanvas.Init(resultItem,materials);
        this.materials = materials;
        this.resultItem = resultItem;
        this.resultNumber = resultNumber;
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
        return inventoryManager.Craft(material,numbers,resultItem,resultNumber);
    }
}
