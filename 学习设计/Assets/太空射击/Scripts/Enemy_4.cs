using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Part另一个可序列化的数据存储类，与WeaponDefinition类似
[System.Serializable]
public class Part
{
    //下面三个字段需要在检视面板中进行定义
    public string name;
    public float health;
    public string[] protectedBy;

    //这两个字段将在Start（）中自动设置
    //像这样的缓存变量可以让程序更快，更容易访问
    public GameObject go;
    public Material mat;
}
public class Enemy_4 :  Enemy {
    //Enemy_4最开始将出现在屏幕之外
    //然后在屏幕内随机选取一个运动终点
    //到达终点之后，它会在屏幕内随机选取另外一个运动终点，直至被玩家消灭
    public Vector3[] points;             //存储插值的p0,p1
    public float timeStart;              //Enemy_4的出生时间
    public float duration = 4;           //Eenmy_4每段运动的时间长度
    public Part[] parts;

	void Start () {
        points = new Vector3[2];
        //Main.SpawnEnemy()中已经选定了一个初始位置
        //所以把这个点作为初始的p0和p1
        points[0] = pos;
        points[1] = pos;

        InitMovement();

        //在parts数组中缓存每个组建的游戏对象和材质
        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
	}

    void InitMovement()
    {
        //在屏幕上选取一个点作为运动终点
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.enemySpawnPadding;
        Bounds cBounds = Utils.camBounds;
        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.x - esp);

        points[0] = points[1];
        points[1] = p1;

        //重置时间
        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);

        pos = (1 - u) * points[0] + u * points[1];
        //base.Move();
    }

    //重写Enemy.cs中的OnCollisionEnter
    //对于MonoBehaviour中OnCollisionEnte()等常规Unity函数
    //这里不需要添加override关键字
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //在进入屏幕之前,敌机不会受到伤害
                //这可以避免玩家射击到屏幕外看不到的敌机
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }

                //给敌机造成伤害
                //找到被击中的游戏对象
                //函数参数Collision collision 中包含一个由碰撞点构成的数组contacts[]
                //因为发生了碰撞，我们可以确信数组中至少存在一个元素contacts[0]
                //碰撞点中包含了对碰撞器thisCollider的引用
                //该碰撞器为Enemy_4被击中组件的碰撞器
                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit.protectedBy != null)  //如果未找到被击中的组件prtHit
                {
                    //通常是因为contact[0]中的thisCollider不是敌机的组件
                    //而是ProjectileHero
                    //这时,只需要查看参与碰撞的另一个碰撞器otherCollider
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //检查该组件是否受到保护
                if (prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        //如果保护它的组件还未被摧毁.....
                        if (!Destroyed(s))
                        {
                            //则暂时不对该组件造成伤害
                            Destroy(other);         //消除炮弹ProjectileHero
                            return;                 //在造成伤害之前返回
                        }
                    }
                }
                //如果它未被保护，则会受到保护
                //根据炮弹类型Projectile.type和字典Main.W_DEFS得到伤害值
                prtHit.health -= Main.W_DEFS[p.type].damageOnHit;
                //在组件上显示伤害效果
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    //禁用被伤害的组件，而不是消灭整架敌机
                    prtHit.go.SetActive(false);
                }
                //查看是否整架敌机已被消灭
                bool allDestroyed = true;
                foreach(Part prt in parts)
                {
                    if (!Destroyed(prt))             //如果有一个组件存在
                    {
                        allDestroyed = false;        //则将allDestroyed设置为false
                        break;                       //并跳出foreach循环
                    }
                }
                if (allDestroyed)
                {
                    //如果它被完全消灭
                    //通知Main单例对象该敌机被消灭
                    Main.S.ShipDestroyed(this);
                    //消灭敌机
                    Destroy(this.gameObject);
                }
                Destroy(other);                       //消除炮弹
                break;
        }

    }
    Part FindPart(string  n)
    {
        foreach(Part prt in parts)
        {
            if(prt.name==n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go==go)
            {
                return (prt);
            }
        }
        return (null);
    }

    //判断是否被摧毁
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            //如果传入的参数不是真正的组件
            return (true);     //返回true（表示它确实以被摧毁）
        }
        //返回prt.health<=0的比较结果
        //如果组件的生命值prt.health小于或等于0，则返回true(表示已被摧毁)
        return (prt.health <= 0);
    }
    //该函数将改变组件颜色，不是整架敌机
    void ShowLocalizedDamage(Material n)
    {
        n.color = Color.red;
        remainingDamageFrames = showDamageFrames;

    }
}
