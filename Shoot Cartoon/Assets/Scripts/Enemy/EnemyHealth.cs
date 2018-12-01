using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
 *类EnemyHealth
 * 挂在所有Enemey上，实现怪物受伤、死亡、消失
 * 公共变量startingHealth决定怪物的血量
 * 公共变量sinkSpeed决定怪物死亡后消失速度
 * 公共变量scoreValue决定怪物分值
 * 公共变量deathClip决定怪物死亡音效
 * 公共方法TakeDamage(int amount,Vector3 hitPoint)，判定怪物受伤位置以及伤害
 * 公共方法StartSinking()怪物消失以及得分
 */
public class EnemyHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public GameObject[] drop;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentHealth = startingHealth;
    }

    private void Update()
    {
        if(isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }
    //怪物受伤及受伤位置
    public void TakeDamage(int amount,Vector3 hitPoint)
    {
        if (isDead) return;
        enemyAudio.Play();
        currentHealth -= amount;
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        TakeDamage(20, other.transform.position);
    }
    //死亡
    void Death()
    {
        isDead = true;
        //死亡后碰撞器改为触发，使其可以下沉
        capsuleCollider.isTrigger = true;
        anim.SetTrigger("Dead");
        int i = Random.Range(0, 10);
        if(i<=5){ }
        else if (i <= 7)
        {
            Instantiate(drop[0], new Vector3(transform.position.x, 0.5f, transform.position.z), transform.rotation);
        }
        else if (i <= 9)
        {
            Instantiate(drop[Random.Range(1, 2)], new Vector3(transform.position.x, 0.5f, transform.position.z), transform.rotation);
        }
        else
        {
            Instantiate(drop[3], new Vector3(transform.position.x,0.5f,transform.position.z), transform.rotation);
        }
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }
    //死亡下沉消失，得分
    public void StartSinking()
    {
        //关闭网格导航
        GetComponent<NavMeshAgent>().enabled = false;
        //开启IK运动学，因为开始设置时锁定了Y轴，无法下落，开启运动学后不再受其影响
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }
}
