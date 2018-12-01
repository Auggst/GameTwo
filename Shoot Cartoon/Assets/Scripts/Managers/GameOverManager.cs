using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 *类GameOverManager
 * 管理游戏结束与重新开始
 * 公共变量restartDelay重开时间
 */
public class GameOverManager : MonoBehaviour {
    public PlayerHealth playerHealth;
    public float restartDelay = 5f;

    Animator anim;
    float restartTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        gameover();
    }
    public void gameover()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");

            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
