using System;
using UnityEngine;
using TMPro;

public class CraftingElement : MonoBehaviour
{
    [Serializable]
    public struct CraftingMaterial
    {
        public Item Material;
        public int number;
    }
    public CraftingMaterial[] Materials;
    public Item_Weapon ResultItem;
    public int ResultNumber;
    private CraftingStation craftingStation;

    void Start()
    {
        craftingStation = transform.parent.GetComponent<CraftingStation>();
    }

    public void OnClick()
    {
        craftingStation.ShowDescription(Materials,ResultItem,ResultNumber);
    }
}   
