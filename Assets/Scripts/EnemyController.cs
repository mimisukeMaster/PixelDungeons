using UnityEngine;

public class EnemyController : MonoBehaviour
{    


    public float WonderForce = 5.0f;

    public float ChasingSpeed = 8.0f;

    public float DetectionInterval = 0.5f;

    public float DetectionDistance = 5.0f;


    private Rigidbody rb;

    private float nextRayTime;

    private bool isChasing;

    private GameObject chasingTarget;



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

 　　　　//追跡処理
　　if(isChasing){
    Vector3 dirVector = (chasingTarget.transform.position-transform.position).normalized;
    rb.linearVelocity = dirVector * ChasingSpeed;


}



        // ランダムに移動
        if (rb.linearVelocity.magnitude < 0.1f){
            Vector3 randomDirection = new Vector3(
        Random.Range(-1f,1f),
        0f,
        Random.Range(-1f,1f)
        ).normalized;

        rb.AddForce(randomDirection * WonderForce,ForceMode.Impulse);



        }
        
    }
   

    private void DetectPlayer() {
        // Rayを飛ばす
       if( Physics.Raycast(
            transform.position, transform.forward, out RaycastHit hit, DetectionDistance)) {
                
            if (hit.collider.CompareTag("Player")) {
                // プレイヤー検出時にデバッグ用にRayを可視化
                Debug.DrawLine(transform.position, hit.point, Color.red, 1f);
                // デバッグ用にRayを可視化
                isChasing = true;
            }
            else isChasing = false;     
        }   
    }
}
