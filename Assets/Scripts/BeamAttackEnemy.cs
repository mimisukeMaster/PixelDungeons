using System.Collections;
using UnityEngine;

public class BeamAttackEnemy : EnemyController
{
    [Space(20)]
    [Tooltip("ビームの攻撃間隔")]
    public float BeamInterval = 5.0f;
    [Tooltip("ビームの長さ")]
    public float BeamLength = 20.0f;

    private float nextBeamTime;
    private LineRenderer lineRenderer;


    protected override void Start()
    {
        base.Start();
        BeamInit();
    }

    protected override void Update()
    {
        base.Update();

        // 一定時間ごとに攻撃挙動
        if(Time.time > nextBeamTime && isChasing)
        {
            StopCoroutine(BeamAttack());
            StartCoroutine(BeamAttack());
            nextBeamTime = Time.time + BeamInterval;
        }
    }

    /// <summary>
    /// 予測線の初期設定
    /// </summary>
    private void BeamInit()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;
    }
    
    /// <summary>
    /// ビーム攻撃コルーチン
    /// </summary>
    private IEnumerator BeamAttack()
    {
        lineRenderer.enabled = true;
        
        // 予測線の準備
        SetBeamAppearance(0.5f, 0.5f, Color.cyan, Color.blue);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + distanceVector.normalized * BeamLength);

        yield return new WaitForSeconds(2.0f);

        // 実際に攻撃
        SetBeamAppearance(1.0f, 2.0f, Color.red, Color.yellow);

        yield return new WaitForSeconds(2.0f);

        lineRenderer.enabled = false;
    }

    /// <summary>
    /// ビームの外観を設定
    /// </summary>
    private void SetBeamAppearance(float startWidth, float endWidth, Color startColor, Color endColor)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }

    public override void OnDied()
    {
        if(Random.Range(0f, 1.0f) < DropProbability) Instantiate(DropItem, transform.position, Quaternion.identity); 
        base.OnDied();
    }
}
