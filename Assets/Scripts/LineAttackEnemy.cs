using UnityEngine;

public class LineAttackEnemy : EnemyController
{
    [Space(20)]
    public float WaveInterval = 5.0f;

    private float nextWaveTime;
    private LineRenderer lineRenderer;


    protected override void Start()
    {
        base.Start();

        // 線の描画の準備
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    protected override void Update()
    {
        // 敵のUpdate()を実行
        base.Update();

        // 攻撃処理
        if(Time.time > nextWaveTime && isChasing)
        {
            // 攻撃線の始点と終点を設定
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, chasingTarget.transform.position);  
            nextWaveTime = Time.time + WaveInterval;
        }
        nextWaveTime = Time.time + WaveInterval;

    }
}
