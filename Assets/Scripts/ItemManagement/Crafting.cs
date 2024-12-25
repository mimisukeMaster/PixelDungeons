using System;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public InventoryManager inventoryManager;

    [Serializable]
    public struct CraftingMaterial
    {
        public Item Material;
        public int number;
    }
    public CraftingMaterial[] Materials;
    public Item ResultItem;
    public int ResultNumber;

    public void OnClick()
    {
        Item[] material = new Item[Materials.Length ];
        int[] numbers = new int[Materials.Length];

        for(int i = 0;i < Materials.Length; i ++)
        {
            material[i] = Materials[i].Material;
            numbers[i] = Materials[i].number;
        }
        inventoryManager.Craft(material,numbers,ResultItem,ResultNumber);
    }
}   
