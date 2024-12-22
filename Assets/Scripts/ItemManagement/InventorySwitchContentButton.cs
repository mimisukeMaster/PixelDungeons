using UnityEngine;

public class InventorySwitchContentButton : MonoBehaviour
{
    public GameObject material;
    public GameObject weapon;
    public GameObject consumable;

    public GameObject target;

    private void Start() 
    {
        material.SetActive(false);
        weapon.SetActive(false);
        consumable.SetActive(false);
        material.SetActive(true);    
    }

    public void OnClick()
    {
        material.SetActive(false);
        weapon.SetActive(false);
        consumable.SetActive(false);
        target.SetActive(true);
    }
}
