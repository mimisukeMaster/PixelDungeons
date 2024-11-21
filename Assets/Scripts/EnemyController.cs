using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float DetectionInterval = 0.5f;
    public float DetectionDistance = 5.0f;

    private Rigidbody rb;
    private float nextRayTime;
    private bool isChasing;

    private void Start()
    {
        nextRayTime = 0;
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        // 一定時間ごとにプレイヤーを検知
        if (Time.time > nextRayTime) {
            DetectPlayer();
            nextRayTime = Time.time + DetectionInterval;
        }

        // ランダムに移動
        
    }

    private void DetectPlayer() {
        // Rayを飛ばす
        Physics.Raycast(
            transform.position, transform.forward, out RaycastHit hit, DetectionDistance);
        
        if (hit.collider.CompareTag("Player")) {
             // プレイヤー検出時にデバッグ用にRayを可視化
            Debug.DrawLine(transform.position, hit.point, Color.red, 1f); // デバッグ用にRayを可視化
            isChasing = true;
        }
        else isChasing = false;
    }
}
