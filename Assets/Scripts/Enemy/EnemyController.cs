using UnityEngine;

/// <summary>
/// 敵の基本共通処理
/// </summary>
/// <remarks>プレイヤー検知フラグ<c>isChasing</c>を用い継承先で制御する</remarks>
public class EnemyController : MonoBehaviour
{
    [Tooltip("ランダムな方向に動く時の初速")]
    public float WanderVelocity = 5.0f;
    [Tooltip("プレイヤーを追跡する速さ")]
    public float ChasingSpeed = 4.0f;
    [Tooltip("ランダムな方向に動く時間間隔")]
    public float WanderInterval = 1.0f;
    [Tooltip("プレイヤーを検知する時間間隔")]
    public float DetectionInterval = 0.5f;
    [Tooltip("プレイヤーを発見するまでの距離")]
    public float DetectionRadius = 5.0f;
    [Tooltip("攻撃力")]
    public int Attack = 10;
    [Tooltip("ドロップアイテム")]
    public GameObject DropItem;
    [Tooltip("アイテムドロップの確率")]
    public float DropProbability = 1.0f;

    protected Rigidbody rb;
    protected float nextDetectTime;
    protected float nextWanderTime;
    protected bool isChasing;
    protected GameObject targetPlayer;
    protected Vector3 distanceVector;
    protected SpawnManager spawnManager;


    protected virtual void Start()
    {
        nextDetectTime = Time.time;
        nextWanderTime = Time.time;
        rb = GetComponent<Rigidbody>();
        spawnManager = GameObject.Find("SpawnManager")?.GetComponent<SpawnManager>();
    }

    protected virtual void Update()
    {
        // 一定時間ごとにプレイヤー検知
        DetectPlayer();

        // 平常時のランダム移動
        RandomMove();
    }

    /// <summary>
    /// プレイヤーの検知処理
    /// </summary>
    protected void DetectPlayer()
    {
        if (Time.time < nextDetectTime) return;

        // 周囲の物体を検知
        Collider[] detectedObjs = Physics.OverlapSphere(transform.position, DetectionRadius);
        isChasing = false;

        foreach (var obj in detectedObjs)
        {
            if (obj.CompareTag("Player"))
            {
                // 衝突相手を格納
                targetPlayer = obj.gameObject;

                // 距離ベクトルを計算
                distanceVector = targetPlayer.transform.position - transform.position;

                isChasing = true;
                break;
            }
        }

        if (!isChasing) targetPlayer = null;
        nextDetectTime = Time.time + DetectionInterval;
    }

    /// <summary>
    /// ランダム移動処理
    /// </summary>
    private void RandomMove()
    {
        if (Time.time < nextWanderTime || isChasing || rb.isKinematic) return;

        Vector3 randomDirection = new Vector3(
                Random.Range(-1.0f, 1.0f), 0f, Random.Range(-1.0f, 1.0f)).normalized;
        rb.linearVelocity = randomDirection * WanderVelocity;

        // 向きを合わせる
        transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);

        nextWanderTime = Time.time + WanderInterval;
    }

    /// <summary>
    /// 死んだときの処理
    /// </summary>
    public virtual void OnDied()
    {
        if (Random.Range(0f, 1.0f) < DropProbability)
        {
            GameObject dropItem = Instantiate(DropItem, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        }
        SpawnManager.EnemiesInStage.Remove(gameObject);
        Destroy(gameObject);
    }

}
