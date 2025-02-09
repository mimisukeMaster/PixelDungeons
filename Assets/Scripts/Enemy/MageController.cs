using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MageController : EnemyController
{
    public float TeleportDistance;
    float nextTeleportTime;
    public float TeleportInterval=1.0f;
    public GameObject magic;
    public float cooldown;
    public float range;
    public float speed;
    public Transform TeleportPosition;
    public ParticleSystem teleportEffect;

    protected override void Start()
    {
        base.Start();
        nextTeleportTime=Time.time;
    }

    protected override void Update()
    {
        DetectPlayer();
        if(isChasing){
            if (Time.time < nextTeleportTime) return;

            Vector3 teleportPosition=new Vector3(TeleportDistance,0,0);
            teleportPosition=Quaternion.Euler(0,UnityEngine.Random.Range(0,360),0)*teleportPosition;
            teleportPosition+=targetPlayer.transform.position;
            teleportPosition.y=0;

            TeleportPosition.transform.position = transform.position + new Vector3(0,0.5f,0);
            TeleportPosition.transform.rotation = Quaternion.FromToRotation(Vector3.forward,teleportPosition - transform.position);
            teleportEffect.Play();

            transform.position = teleportPosition;
            nextTeleportTime=Time.time+TeleportInterval;
            transform.rotation = Quaternion.LookRotation(targetPlayer.transform.position - transform.position);

            StartCoroutine(WaitCooldown());
        }
    }

    private IEnumerator WaitCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        GameObject newMagic=Instantiate(magic,transform.position+new Vector3(0,0.5f,0),Quaternion.identity);
        newMagic.GetComponent <Rigidbody>().linearVelocity=(targetPlayer.transform.position-(transform.position+ new Vector3(0,0.5f,0))).normalized*speed;
        if(speed!=0)
        {
            newMagic.GetComponent<AttackController>().Init("Player",Damage,range/speed,1);
        }
        else
        {
            newMagic.GetComponent<AttackController>().Init("Player",Damage,5,1);
        }
    }
}

