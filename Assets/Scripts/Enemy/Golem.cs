using UnityEngine;

public class Golem : MiddleBossEnemy
{
    [Tooltip("目のボーン")]
    public Transform Eye;
    [Tooltip("攻撃モーションを始める距離")]
    public float AttackMotionRange = 8.0f;
    [Tooltip("攻撃時間間隔")]
    public float SmashInterval = 1.0f;
    [Tooltip("攻撃の中心")]
    public Transform SmashPosition;
    [Tooltip("攻撃半径")]
    public float SmashRange = 4.0f;
    [Space(20)]
    [Header("ビーム関係")]
    [Tooltip("ビームを出す確率")]
    public float beamProbability = 0.2f;//遠距離の場合一定確率でビームを打つ
    [Tooltip("ビームの攻撃間隔")]
    public float BeamInterval = 5.0f;
    [Tooltip("ビームの長さ")]
    public float BeamLength = 20.0f;
    [Tooltip("ビームのPrefab")]
    public GameObject Beam;
    [Tooltip("ビームが打たれるまでの時間")]
    public float BeamChargeTime = 1;
    [Tooltip("ビームが照射されている時間")]
    public float BeamEmittionTime = 2;
    [Space(20)]
    [Tooltip("近接攻撃で出るがれきの数")]
    public int SmashParticleNumber;
    [Tooltip("がれき")]
    public GameObject SmashParticle;

    private bool isInitChasing = true;

    protected override void Update()
    {
        DetectPlayer();
        if (isChasing)
        {
            // 目をプレイヤーに向け、目の上側がちゃんと上を向くようにする
            if (Vector3.Angle(transform.forward, distanceVector) < 90.0f)
            {
                Eye.transform.rotation = Quaternion.LookRotation(targetPlayer.transform.position - Eye.transform.position);
                Eye.transform.rotation = Quaternion.AngleAxis(90.0f, Eye.transform.right) * Eye.transform.rotation;
            }
            // プレイヤーが正面にいない場合は正面を向く
            else Eye.transform.rotation = Quaternion.Euler(90.0f, 0f, 0f);

            if (isInitChasing)
            {
                CheckNextMove();
                isInitChasing = false;
            }

            // アニメーションの関係でプレイヤーに近づきすぎた時は即座に攻撃の挙動
            if (distanceVector.magnitude < AttackMotionRange)
            {
                animator.StopPlayback();
                animator.SetTrigger("Attack");
            }
        }
    }

    /// <summary>
    /// プレイヤーに十分近づいたかを調べ、移動または攻撃の挙動をする
    /// 2回目以降の呼び出しは<see cref="GolemEventHandler"/>から
    /// </summary>
    public void CheckNextMove()
    {
        if (!isChasing)
        {
            animator.SetTrigger("Idle");
            return;
        }

        Vector3 destVec = distanceVector;
        destVec.y = 0;
        gameObject.transform.forward = destVec;

        if (distanceVector.magnitude < AttackMotionRange)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            if (Random.value < beamProbability) animator.SetTrigger("Beam");
            else animator.SetTrigger("Move");
        }
    }

    public void OnBeamStart()
    {
        // 追跡中でないならreturn
        if (!isChasing) return;

        if (Vector3.Angle(transform.forward, distanceVector) < 90.0f)
        {
            // 目をプレイヤーに向け、目の上側が正しく上を向くようにする　ビームは目の回転をもとにしているので再度回転を調整する
            Eye.transform.rotation = Quaternion.LookRotation(targetPlayer.transform.position - Eye.transform.position);
            Eye.transform.rotation = Quaternion.AngleAxis(90.0f, Eye.transform.right) * Eye.transform.rotation;
        }
        // プレイヤーの方を向いていない場合は正面を向く
        else Eye.transform.rotation = Quaternion.Euler(90.0f, 0f, 0f);

        GameObject beam = Instantiate(Beam, Eye.transform.position, Quaternion.AngleAxis(90.0f, Eye.transform.right) * Eye.transform.rotation);
        beam.GetComponentInChildren<Beam>().BeamInit("Player", Attack, 100, BeamChargeTime, BeamEmittionTime, 20.0f);
    }

    public void OnAttackLand()
    {
        for (int i = 0; i < SmashParticleNumber; i++)
        {
            GameObject particle = Instantiate(SmashParticle, SmashPosition.position + Vector3.up * 0.4f, Quaternion.Euler(Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f)));
            particle.GetComponent<AttackController>().Init("Player", Attack, 2.0f);
            particle.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(3.0f, 8.0f), Random.Range(-10.0f, 10.0f));
        }
    }
}