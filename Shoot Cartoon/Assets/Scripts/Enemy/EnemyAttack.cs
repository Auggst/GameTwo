using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *类EnemyAttack
 * 挂在所有的Enemy上，实现攻击范围判断和攻击
 * 公共变量timeBetweenAttacks定义攻击间隔
 * 公共变量attackDamage定义攻击伤害
 */
public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;//攻击间隔
    public int attackDamage = 10;//攻击伤害

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth=GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }
    //触发器判断攻击范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("PlayerDead");
        }
    }
    //攻击，缺少攻击动画
    void Attack()
    {
        timer = 0f;
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }


}
