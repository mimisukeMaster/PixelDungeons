using UnityEngine;

public class CraftButton : MonoBehaviour
{
    public CraftingStation craftingStation;

    public void OnClick()
    {
        craftingStation.Craft();
    }
}
