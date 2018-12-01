using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    //使用二维向量Vector的x存储在Random.Ranger最小值
    //并用y值存储最大值是一种不常见但很方便的用法
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;              //升级道具存在的时间长度
    public float fadeTime = 4f;              //升级道具渐隐所用的时间
    public bool ____________;
    public WeaponType type;                  //升级道具的类型
    public GameObject cube;                  //对立方体子对象的引用
    public TextMesh letter;                  //对文本网格的引用
    public Vector3 rotPerSecond;             //欧拉旋转的速度
    public float birthTime;

    private void Awake()
    {
        //对立方体的引用
        cube = transform.Find("Cube").gameObject;
        //设置文本网格的引用
        letter = GetComponent<TextMesh>();
        //随机获得一个xyz的速度
        Vector3 vel = Random.onUnitSphere;
        //onUnitSphere,随机获得一个以原点为球心的，1为半径的球表面的任意一点
        vel.z = 0;
        vel.Normalize();
        //三维向量的Normalize方法让它的长度变为1
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        //将速度设置为driftMinMax的x,y值之间的一个值
        GetComponent<Rigidbody>().velocity = vel;
        //将本游戏对象的旋转设置为[0,0,0]
        transform.rotation = Quaternion.identity;
        //Quanternion.identity的旋转为0
        //使用rotMinMax的x,y值设置立方体子对象每秒旋转圈数rotPerSecond
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        //每隔两秒调用一次CheckOffscreen（）检查是否处于屏幕之外
        InvokeRepeating("CheckOffscreen", 2f, 2f);
        birthTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        //每帧旋转Cube
        //将它的旋转角度设置为旋转速度乘以Time.time，使他基于时间旋转
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        //隔一定时间后，让升级道具渐隐
        //根据默认值，升级道具可以存在10秒钟
        //如果u>=1,消除升级道具
        float u = fadeTime / 10;
        if(u>=1)
        {
            Destroy(this.gameObject);
            return;
        }
        //使用变量u确定立方体和文字的不透明度
        if(fadeTime>0)
        {
            Color c = cube.GetComponent<Renderer>().material.color;
            c.a = 1f - u;
            cube.GetComponent<Renderer>().material.color = c;
            //让字母也渐隐.只不过程度不一样
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
	}
    //这个SetType（）与Weapon和Projectile脚本中的SetType（）有所不同
    public  void SetType(WeaponType wt)
    {
        //从Main脚本中获取WeaponDefinition值
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        //设置立方体子对象的颜色
        cube.GetComponent<Renderer>().material.color = def.color;
        letter.text = def.letter;            //设置显示的颜色
        type = wt;                           //最后设置升级道具的类型
    }
    public void AbsorbedBy(GameObject target)
    {
        //Hero类在收集到道具之后调用本函数
        //我们可以让升级道具逐渐缩小，吸收到目标对象中
        //但现在，简单消除this.gameObject即可
        Destroy(this.gameObject);
    }
    void CheckOffscreen()
    {
        //如果升级道具完全漂出屏幕
        if(Utils.ScreenBoundsCheck(cube.GetComponent<Collider>().bounds,BoundsTest.offScreen)!=Vector3.zero)
        {
            //..then destroy this GameObject
            Destroy(this.gameObject);
        }
    }
}
