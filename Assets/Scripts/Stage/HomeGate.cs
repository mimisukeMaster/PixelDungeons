using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeGate : MonoBehaviour
{
    public string TargetScene;

    private void OnCollisionEnter(Collision other) 
    {
        SceneManager.LoadScene(TargetScene);
    }
}
