using UnityEngine;

public class LineAttackEnemy : EnemyController
{
    [Space(20)]
    public float WaveInterval = 5.0f;
    public float WaveLength = 20.0f;

    private float nextWaveTime;
    private LineRenderer lineRenderer;


    protected override void Start()
    {
        base.Start();

        // 線の描画の準備
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    protected override void Update()
    {
        // 敵のUpdate()を実行
        base.Update();

        // 一定時間ごとに弾を出す
        if(Time.time > nextWaveTime && isChasing)
        {
            Vector3 distanceVec = (chasingTarget.transform.position - transform.position).normalized;
            
            // 攻撃線の始点と終点を設定
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + distanceVec * WaveLength);
            nextWaveTime = Time.time + WaveInterval;
        }
    }
}
