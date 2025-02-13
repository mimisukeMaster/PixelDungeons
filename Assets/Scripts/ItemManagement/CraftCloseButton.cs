using UnityEngine;

public class CraftCloseButton : MonoBehaviour
{
    public GameObject craftingDescriptionCanvas;

    public void OnClick()
    {
        craftingDescriptionCanvas.SetActive(false);
    }
}
