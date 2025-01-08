using UnityEngine;

public class HomingAttack : AttackController
{
    private float DetectionFrequency = 0.2f;
    private float homingAngle;
    protected Transform myTransform;
    protected Rigidbody myRb;

    private float nextDetectTime = 0;

    private Vector3 targetPosition;

    private float speed = 1;

    private float detectionDistance;

    public virtual void InitHoming(string TargetTag, int Damage, float destroyTime, int Penetration, float Speed, float HomingAngle, float DetectionDistance)
    {
        base.Init(TargetTag, Damage, destroyTime, Penetration);
        homingAngle = HomingAngle;
        speed = Speed;
        detectionDistance = DetectionDistance * DetectionDistance;//sqrMagnitudeを見るので２乗
        Destroy(gameObject, destroyTime);
        myTransform = transform;
        myRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time > nextDetectTime)
        {
            if (targetTag == "Enemy") targetPosition = DetectEnemy();
            else if (targetTag == "Player") targetPosition = PlayerController.PlayerObject.transform.position;

            nextDetectTime += DetectionFrequency;
        }
        if (targetPosition != Vector3.zero)
        {
            myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation,
                Quaternion.LookRotation(targetPosition - myTransform.position),
                homingAngle * Time.deltaTime);
        }

        myRb.linearVelocity = myTransform.forward * speed;
    }

    private Vector3 DetectEnemy()
    {
        //一番近い敵を識別する
        Vector3 closestEnemyPosition = Vector3.zero;
        float sqrClosestDistance = detectionDistance;//弾から一番近い敵までの距離
        foreach (GameObject enemy in SpawnManager.EnemiesInStage)
        {
            //敵までの距離
            float distance = (myTransform.position - enemy.transform.position).sqrMagnitude;
            if (distance < sqrClosestDistance)
            {
                closestEnemyPosition = enemy.transform.position;
                sqrClosestDistance = distance;
            }
        }
        Debug.Log(sqrClosestDistance);
        return closestEnemyPosition;
    }
}
