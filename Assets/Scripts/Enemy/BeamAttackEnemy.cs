using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class BeamAttackEnemy : EnemyController
{
    [Space(20)]
    [Tooltip("ビームの攻撃間隔")]
    public float BeamInterval = 5.0f;
    [Tooltip("ビームの長さ")]
    public float BeamLength = 20.0f;
    [Tooltip("ビームのPrefab")]
    public GameObject Beam;
    [Tooltip("浮遊高度")]
    public float Altitude = 2.0f;

    private float nextBeamTime;
    private LineRenderer lineRenderer;
    private GameObject beamObj;


    protected override void Start()
    {
        base.Start();
        BeamInit();

        // 位置と回転の固定
        transform.position = new Vector3(transform.position.x, Altitude, transform.position.z); 
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
        rb.isKinematic = true;
        lineRenderer.enabled = true;
        
        // 予測線の準備
        SetBeamAppearance(0.5f, 0.5f, Color.cyan, Color.blue);
        Vector3 beamRootPos = transform.position;
        Vector3 beamTipPos =  transform.position + distanceVector.normalized * BeamLength;
        lineRenderer.SetPosition(0, beamRootPos);
        lineRenderer.SetPosition(1, beamTipPos);

        yield return new WaitForSeconds(2.0f);

        // 実際に攻撃
        beamObj = Instantiate(Beam, transform.position, Quaternion.LookRotation(beamRootPos - beamTipPos));
        beamObj.transform.localScale = new Vector3(beamObj.transform.localScale.x, beamObj.transform.localScale.y, BeamLength);
        beamObj.GetComponentInChildren<AttackController>().Init("Player", Attack, 2.0f);

        yield return new WaitForSeconds(2.0f);

        Destroy(beamObj);
        rb.isKinematic = false;
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
        if (beamObj != null) Destroy(beamObj);
        if(Random.Range(0f, 1.0f) < DropProbability) Instantiate(DropItem, transform.position, Quaternion.identity); 
        base.OnDied();
    }
}
