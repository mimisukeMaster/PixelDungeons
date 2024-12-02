using UnityEngine;

/// <summary>
/// 敵の基本共通処理
/// </summary>
/// <remarks>プレイヤー検知フラグ<c>isChasing</c>を用い継承先で制御する</remarks>
public class EnemyController : MonoBehaviour
{
    [Header("ランダムな方向に動く時の初速")]
    public float WanderVelocity = 5.0f;
    [Header("プレイヤーを追跡する速さ")]
    public float ChasingSpeed = 4.0f;
    [Header("ランダムな方向に動く時間間隔")]
    public float WanderInterval = 1.0f;
    [Header("プレイヤーを検知する時間間隔")]
    public float DetectionInterval = 0.5f;
    [Header("プレイヤーを発見するまでの距離")]
    public float DetectionRadius = 5.0f;

    protected Rigidbody rb;
    protected float nextRayTime;
    protected float nextWanderTime;
    protected bool isChasing;
    protected GameObject chasingTarget;
    protected Vector3 detectArea;
    protected Material debugMat;


    protected virtual void Start()
    {
        nextRayTime = 0f;
        nextWanderTime = 0f;
        detectArea = transform.lossyScale;
        rb = GetComponent<Rigidbody>();
        debugMat = GetComponent<MeshRenderer>().material;
    }

    protected virtual void Update()
    {
        // 一定時間ごとにプレイヤーを検知
        if (Time.time > nextRayTime)
        {
            DetectPlayer();
            nextRayTime = Time.time + DetectionInterval;
        }

        // ランダムに移動
        else if (Time.time > nextWanderTime)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1.0f, 1.0f), 0f, Random.Range(-1.0f, 1.0f)).normalized;

            rb.linearVelocity = randomDirection * WanderVelocity;
            nextWanderTime = Time.time + WanderInterval;
        }

    }

    private void DetectPlayer()
    {
        // 周囲の物体を検知
        Collider[] cols = Physics.OverlapSphere(transform.position, DetectionRadius);

        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                debugMat.color = Color.red;
                
                // 衝突相手を格納
                chasingTarget = col.gameObject;
                
                isChasing = true;
                return;
            }
        }
        debugMat.color = Color.white;
        chasingTarget = null;
        isChasing = false;
        
    }
}
