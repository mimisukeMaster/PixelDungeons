using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float WanderVelocity = 5.0f;
    public float ChasingSpeed = 8.0f;
    public float WanderInterval = 1.0f;
    public float DetectionInterval = 0.5f;
    public float DetectionDistance = 10.0f;

    private Rigidbody rb;
    private float nextRayTime;
    private float nextWanderTime;
    private bool isChasing;
    private GameObject chasingTarget;
    private Material debugMat;


    private void Start()
    {
        nextRayTime = 0f;
        nextWanderTime = 0f;
        rb = GetComponent<Rigidbody>();
        debugMat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        // 一定時間ごとにプレイヤーを検知
        if (Time.time > nextRayTime)
        {
            DetectPlayer();
            nextRayTime = Time.time + DetectionInterval;
        }

        // 追跡処理
        if (isChasing)
        {
            Vector3 dirVector = (chasingTarget.transform.position - transform.position).normalized;
            rb.linearVelocity = dirVector * ChasingSpeed;
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
        // Rayを飛ばす
        if (Physics.Raycast(
            transform.position, transform.forward, out RaycastHit hit, DetectionDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                debugMat.color = Color.red;
                
                // 衝突相手を格納
                chasingTarget = hit.collider.gameObject;
                isChasing = true;
            }
            else isChasing = false;
        }
    }
}
