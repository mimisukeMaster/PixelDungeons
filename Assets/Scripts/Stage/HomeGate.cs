using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeGate : MonoBehaviour
{
    public string TargetScene;
    public AudioClip GateSE;

    private void OnCollisionEnter(Collision other) 
    {
        GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>().PlayOneShot(GateSE);

        SceneManager.LoadScene(TargetScene);
    }
}
