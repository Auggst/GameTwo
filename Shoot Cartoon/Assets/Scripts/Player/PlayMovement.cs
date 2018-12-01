using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMovement : MonoBehaviour
{
    public float speed = 6f;            // 移动速度
    public bool ______________;         //分割线
    Vector3 movement;
    Animator anim;                      // 动画控制器
    Rigidbody playerRigidbody;          // 刚体
    int floorMask;                      // Floor对象层级
    float camRayLength = 100f;          // 摄像机射线距离

    void Awake()
    {
        // 获得Floor层级
        floorMask = LayerMask.GetMask("Floor");

        // 初始化
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // 获取输入
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 调用Move()方法移动角色
        Move(h, v);

        // 调用Turning()方法转向
        Turning();

        // 运动角色
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        // 设置movement
        movement.Set(h, 0f, v);

        // 设置方向和移动
        movement = movement.normalized * speed * Time.deltaTime;

        // 移动到当前位置
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // 用鼠标控制摄像机的中心射线
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 创建一个RacycastHit变量存储射线发生碰撞的地方
        RaycastHit floorHit;

        // 如果发生碰撞到floorMask层级
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // 保存二者间距离
            Vector3 playerToMouse = floorHit.point - transform.position;

            // 确保y变量为0
            playerToMouse.y = 0f;

            // 利用方法存储旋转信息
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // 旋转
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // 只要获取到输入就移动
        bool walking = h != 0f || v != 0f;

        // 播放移动动画
        anim.SetBool("IsWalk", walking);
    }
}