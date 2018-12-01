using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Enemy_1为Enemy的派生类
public class Enemy_1 : Enemy {
    //因为Enemy_1是Enemy的派生类，______布尔变量不会起作用
    //完成一个完整的正弦曲线周期所需的时间
    public float waveFrequency = 2;
    //正弦曲线的宽度，以米为单位
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float x0 = -12345;          //初始位置x的坐标
    private float birthTime;
	
	void Start () {
        //将x0设置为Enemy_1的初始x坐标
        //这句代码可以正常工作，因为Start()之前的Main.SpawnEnemy()
        //已经完成了对位置的设置
        //(但如果在Awake()中使用这句代码就会过早！)
        //这句代码可以正常工作的另外一个原因是Enemy脚本中没有Start()方法
        x0 = pos.x;
        birthTime = Time.time;
	}
    //重写Enemy的Move函数
    public override void Move()
    {
        //因为pos是一种属性，不能直接设置pos.x
        //所以将pos赋给一个可以修改的三维向量变量
        Vector3 tempPos = pos;
        //基于时间调整theta值
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;
        //让对象绕y轴旋转
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        //对象在y方向上的运动仍由base.Move()函数处理
        base.Move();
    }
}
