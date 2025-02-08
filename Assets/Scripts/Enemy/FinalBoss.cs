using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : EnemyController
{
    public List<FinalBossChild> finalBossChildren = new List<FinalBossChild>();
    public HPController hPController;
    public GameObject shield;
    [Header("ランダムショット")]
    public Coroutine randomShotCoroutine;
    public int randomShotDamage;
    public GameObject randomShotPrefab;

    public Transform estimatedPlayerPositionObject;

    protected override void Start() 
    {
        base.Start();
        hPController.canBeDamaged = false;
        shield.SetActive(false);
        foreach(FinalBossChild finalBossChild in finalBossChildren)
        {
            finalBossChild.Init(this);
        }
    }

    protected override void Update() 
    {
        DetectPlayer();
    }

    public void OnChildDestroyed(FinalBossChild finalBossChild)
    {
        finalBossChildren.Remove(finalBossChild);
        switch(finalBossChildren.Count)
        {
            case 0:
                hPController.canBeDamaged = true;
                shield.SetActive(false);
                StopCoroutine(randomShotCoroutine);
                randomShotCoroutine = StartCoroutine(RandomShot(20,0.3f,20,15));
                break;
            case 1:
                StopCoroutine(randomShotCoroutine);
                randomShotCoroutine = StartCoroutine(RandomShot(20,0.3f,20,15));
                break;
            case 2: 
                StopCoroutine(randomShotCoroutine);
                randomShotCoroutine = StartCoroutine(RandomShot(10,0.3f,20,15));
                break;
            case 3:
                randomShotCoroutine = StartCoroutine(RandomShot(5,0.5f,20,10));
                break;
            case 4:

                break;
        }
        Debug.Log("Stage"+finalBossChildren.Count);
    }

    private IEnumerator RandomShot(int number,float fireRate,float spread,float speed)
    {
        Debug.Log("RandomShot");
        WaitForSeconds waitForCoolDown = new WaitForSeconds(fireRate);
        while(true)
        {
            PlayerController playerController = targetPlayer.GetComponent<PlayerController>();
            Vector3 estimatedPlayerPosition = targetPlayer.transform.position + playerController.lastMoveDirection * (targetPlayer.transform.position - transform.position).magnitude / (speed/10);
            estimatedPlayerPositionObject.position = estimatedPlayerPosition;
            for(int i = 0;i < number;i++)
            {
                //攻撃する方向を決める
                Quaternion randomDirection = Quaternion.Euler(Random.Range(-spread,spread),
                                                        Random.Range(-spread,spread),
                                                        Random.Range(-spread,spread));
                GameObject bullet = Instantiate(randomShotPrefab,transform.position,Quaternion.identity);
                bullet.GetComponent<AttackController>().Init("Player",randomShotDamage,100,10);
                bullet.GetComponent<Rigidbody>().linearVelocity = randomDirection * ((estimatedPlayerPosition-transform.position).normalized*speed);
            }
            yield return waitForCoolDown;
        }
    }

    public override void OnDied()
    {
        base.OnDied();

    }
}
