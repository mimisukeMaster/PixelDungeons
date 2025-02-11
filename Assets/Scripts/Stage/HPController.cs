using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HPController : MonoBehaviour
{   
    [Tooltip("現在の体力")]
    public int HP;
    [Tooltip("体力の最大値")]
    public int MaxHP = 100;
    [Tooltip("TagがPlayer出ない場合は必要なし")]
    public PlayerController PlayerController;
    [Tooltip("プレイヤーがダメージを受けたら発生するエフェクトのUI")]
    public Animator playerDamageUI;
    [Tooltip("プレイヤーの無敵時間")]
    public float PlayerInvincibleTime;
    public bool canBeDamaged = true;
    [Tooltip("TagがEnemyでない場合は必要なし")]
    public EnemyController EnemyController;
    [Tooltip("ダミーでない場合必要なし")]
    public Dummy dummy;
    [Tooltip("HPバー")]
    public Image HPBar;
    [Tooltip("HPテキスト")]
    public TMP_Text HPText;


    void Start()
    {
        HP = MaxHP;
        UpdateHPBar();
    }


    public void Damaged(int damage,Vector3 UIPosition)
    {
        if(!canBeDamaged)return;

        if(dummy != null)
        {
            dummy.damageTaken += damage;
            return;
        }

        HP -= damage;

        UpdateHPBar();
        //プレイヤーがダメージを受けるとUIを表示
        if(gameObject.tag == "Player")
        {
            playerDamageUI.SetTrigger("Damage");
            StartCoroutine(PlayerInvincible());
        }

        if(gameObject.tag == "Enemy")
        {
            // UIを表示
            DamageNumberManager.AddUI(damage, UIPosition);
        }

        if(HP <= 0)
        {
            if(gameObject.CompareTag("Player"))PlayerController.OnDied();
            else if(gameObject.CompareTag("Enemy")) EnemyController.OnDied(gameObject);
        }
    }

    private IEnumerator PlayerInvincible()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(PlayerInvincibleTime);
        canBeDamaged = true;
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
        if (HPBar) HPBar.fillAmount = (float)HP / MaxHP;
        if (HPText != null) HPText.text = $"{HP}/{MaxHP}";
    }
}
