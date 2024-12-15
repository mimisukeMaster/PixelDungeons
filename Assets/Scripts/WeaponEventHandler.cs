using UnityEngine;

public class WeaponEventHandler : MonoBehaviour
{
    private AttackController attackController;
    private PlayerController playerController;
    public GameObject attack;
    public Transform parent;

    private void Start()
    {
        attackController = GetComponentInChildren<AttackController>();
        playerController = GetComponentInParent<PlayerController>();
    }

    /// <summary>
    /// 武器の攻撃イベント発生時の処理
    /// </summary>
    /// <remarks>アニメーションのタイミングでUnity側から呼ばれる</remarks>
    /// 近遠両方で呼ばないといけなく、nullチェックを通して近距離攻撃を行う
    public void AttackEvent()
    {
        GameObject collider = Instantiate(attack,transform.position,parent.rotation);   
        collider.GetComponent<AttackController>().Init("Enemy",10,10,5.0f);
    }
}
