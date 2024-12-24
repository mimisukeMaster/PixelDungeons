using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    public GameObject craftingCanvas;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            craftingCanvas.SetActive(true);
            Time.timeScale = 0.2f;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            craftingCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
