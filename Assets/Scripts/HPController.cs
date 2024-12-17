using JetBrains.Annotations;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{   
    [Tooltip("現在の体力")]
    public int HP;
    [Tooltip("体力の最大値")]
    public int MaxHP = 100;
    [Tooltip("TagがPlayer出ない場合は必要なし")]
    public PlayerController PlayerController;
    [Tooltip("TagがEnemyでない場合は必要なし")]
    public EnemyController EnemyController;
    [Tooltip("HPバー")]
    public Image HPBar;


    void Start()
    {
        HP = MaxHP;
        UpdateHPBar();
    }

    void Update()
    {
        
    }

    public void Damaged(int damage)
    {
        HP -= damage;
        UpdateHPBar();
        if(HP <= 0)
        {
            if(gameObject.CompareTag("Player"))PlayerController.OnDied();
            else if(gameObject.CompareTag("Enemy")) EnemyController.OnDied();
        }
    }

    public void Healed(int heal)
    {
        HP += heal;
        if(HP > MaxHP) HP = MaxHP;
    }

    /// <summary>
    /// HPバーを更新する
    /// </summary>
    public void UpdateHPBar()
    {
        if(HPBar)
        {
            HPBar.fillAmount = (float)HP/MaxHP;
        }
    }
}
