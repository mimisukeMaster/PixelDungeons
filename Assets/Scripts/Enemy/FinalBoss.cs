using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : EnemyController
{
    [Space(20)]
    public List<FinalBossChild> finalBossChildren = new List<FinalBossChild>();
    public GameObject shield;
    public HPController hPController;
    [Header("ランダムショット")]
    public Coroutine randomShotCoroutine;
    public int randomShotDamage;
    public GameObject randomShotPrefab;
    public AudioClip ShotSE;
    [Space(10)]
    [Tooltip("デバッグ用:スタート時卵を倒した状態にする")]
    public bool BossInstantKill;

    public Transform estimatedPlayerPositionObject;
    [Header("死亡処理")]
    public ParticleSystem destroyParticle;
    public float crackTime;
    public Material crackMaterial;
    public MeshRenderer eggMeshRenderer;
    [Tooltip("ドラゴンのプレハブ")]
    public GameObject Dragon;
    private bool isAttackMode;
    private bool isDead = false;
    private AudioSource audioSource;
    private bool isShotSE;

    protected override void Start() 
    {
        base.Start();
        if(BossInstantKill)
        {
            Debug.Log("卵即殺す");
            OnDied(gameObject);
            return;
        }
        CheckState();
        isAttackMode = true;
        hPController.canBeDamaged = false;
        shield.SetActive(true);
        audioSource = GameObject.FindWithTag("AudioSource").GetComponent<AudioSource>();

        if(finalBossChildren.Count == 0)return;
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
        CheckState();
    }

    private void CheckState()
    {
        switch(finalBossChildren.Count)
        {
            case 0:
                OnAllChildrenDead();
                if(randomShotCoroutine != null)StopCoroutine(randomShotCoroutine);
                randomShotCoroutine = StartCoroutine(RandomShot(20,0.3f,20,15));
                break;
            case 1:
                if(randomShotCoroutine != null)StopCoroutine(randomShotCoroutine);
                randomShotCoroutine = StartCoroutine(RandomShot(20,0.3f,20,15));
                break;
            case 2: 
                if(randomShotCoroutine != null)StopCoroutine(randomShotCoroutine);
                randomShotCoroutine = StartCoroutine(RandomShot(10,0.3f,20,15));
                break;
            case 3:
                randomShotCoroutine = StartCoroutine(RandomShot(5,0.5f,20,10));
                break;
            case 4:

                break;
        }
        Debug.Log("Final Boss Stage:"+finalBossChildren.Count);
    }

    private void OnAllChildrenDead()
    {
        Debug.Log("All Childeren Destroyed");
        hPController.canBeDamaged = true;
        shield.SetActive(false);
    }

    private IEnumerator RandomShot(int number,float fireRate,float spread,float speed)
    {
        WaitForSeconds waitForCoolDown = new WaitForSeconds(fireRate);
        while(true && isAttackMode)
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
                GameObject bullet = Instantiate(randomShotPrefab,transform.position + Vector3.up * 2,Quaternion.identity);
                bullet.GetComponent<AttackController>().Init("Player",randomShotDamage,100,1);
                bullet.GetComponent<Rigidbody>().linearVelocity = randomDirection * ((estimatedPlayerPosition-transform.position).normalized*speed);
            
                if (!isShotSE) StartCoroutine(PlayShotSE());
            }
            yield return waitForCoolDown;
        }
    }
    private IEnumerator PlayShotSE()
    {
        isShotSE = true;
        audioSource.PlayOneShot(ShotSE, 0.8f);
        yield return new WaitForSeconds(0.5f);
        isShotSE = false;
    }

    public override void OnDied(GameObject gameObject)
    {
        if(!isDead)
        {
            StartCoroutine(Crack());
            isDead = true;
        }
    }

    private IEnumerator Crack()
    {
        isAttackMode = false;
        hPController.canBeDamaged = false;
        eggMeshRenderer.material = crackMaterial;
        Destroy(hPController.HPBar.transform.parent.parent.gameObject); // HPバーを消す
        yield return new WaitForSeconds(crackTime);

        destroyParticle.Play();
        GameObject dragon = Instantiate(Dragon,transform.position + new Vector3(0,2,1),Quaternion.Euler(-90,0,0));
        SpawnManager.EnemiesInStage.Add(dragon);
        Destroy(eggMeshRenderer.gameObject);
        yield return new WaitForSeconds(5); // 卵が壊れるエフェクトが消えるまで待機

        base.OnDied(transform.parent.gameObject);
    }
}
