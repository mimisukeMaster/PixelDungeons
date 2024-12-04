using System.Collections;
using UnityEngine;

public class BeamAttackEnemy : EnemyController
{
    [Space(20)]
    public float BeamInterval = 5.0f;
    public float BeamLength = 20.0f;

    private float nextBeamTime;
    private LineRenderer lineRenderer;


    protected override void Start()
    {
        base.Start();

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        BeamInit();
    }

    // 予測線の初期化
    private void BeamInit() {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.blue;
        lineRenderer.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        // 一定時間ごとに攻撃挙動
        if(Time.time > nextBeamTime && isChasing)
        {
            StopCoroutine(AttackProcess());
            StartCoroutine(AttackProcess());
            nextBeamTime = Time.time + BeamInterval;
        }
    }

    private IEnumerator AttackProcess() {

        lineRenderer.enabled = true;
        
        // 攻撃の予測線を出す
        Vector3 distanceVec = (chasingTarget.transform.position - transform.position).normalized;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + distanceVec * BeamLength);

        yield return new WaitForSeconds(2.0f);

        // 実際に攻撃する動き
        lineRenderer.startWidth = 1.0f;
        lineRenderer.endWidth = 2.0f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;

        yield return new WaitForSeconds(2.0f);
        BeamInit();
    }
}
