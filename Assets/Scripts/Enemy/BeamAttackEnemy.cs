using System.Collections;
using NUnit.Framework.Constraints;
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
    [Tooltip("ビームが打たれるまでの時間")]
    public float BeamChargeTime = 1;
    [Tooltip("ビームが照射されている時間")]
    public float BeamEmissionTime = 2;

    private float nextBeamTime;
    private LineRenderer lineRenderer;


    protected override void Start()
    {
        base.Start();

        // 位置と回転の固定
        transform.position = new Vector3(transform.position.x, Altitude, transform.position.z);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    protected override void Update()
    {
        base.Update();

        // 一定時間ごとに攻撃挙動
        if (Time.time > nextBeamTime && isChasing)
        {
            GameObject beam = Instantiate(Beam, transform.position, Quaternion.LookRotation(distanceVector * -1.0f));
            LineRenderer line = beam.GetComponentInChildren<LineRenderer>();
            beam.GetComponentInChildren<AttackController>().Init("Player",  float.PositiveInfinity,Damage,line,  BeamChargeTime, BeamEmissionTime, 20.0f);
            StartCoroutine(FreezeForCharge());
            nextBeamTime = Time.time + BeamInterval;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator FreezeForCharge()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(BeamChargeTime + BeamEmissionTime);
        rb.isKinematic = false;
    }

    public override void OnDied(GameObject gameObject)
    {
        if (Random.Range(0f, 1.0f) < DropProbability) Instantiate(DropItem, transform.position, Quaternion.identity);
        base.OnDied(gameObject);
    }
}
