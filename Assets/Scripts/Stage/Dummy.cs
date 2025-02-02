using System.Collections;
using UnityEngine;
using TMPro;

public class Dummy : MonoBehaviour
{
    [Tooltip("テキストを更新するインターバル")]
    public float interval;
    private WaitForSeconds waitForUpdate;
    [HideInInspector]
    public int damageTaken;
    public TMP_Text text;

    private void Start() 
    {
        waitForUpdate = new WaitForSeconds(interval);
        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        while(true)
        {
            yield return waitForUpdate;
            if(damageTaken != 0)text.text = damageTaken.ToString();
            else text.text = "";
            damageTaken = 0;
        }
    }
}
