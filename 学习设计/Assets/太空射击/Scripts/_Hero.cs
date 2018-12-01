using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class _Hero : MonoBehaviour {
    static public _Hero S;
    public float gameRestartDelay = 2f;
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    [SerializeField] private float _shieldLevel = 1;
    public Weapon[] weapons;
    public bool _______;
    public Bounds bounds;
    //声明委托类型
    public delegate void WeaponFireDelegate();
    //创建一个名为fireDelegate的WeaponFireDelegate类型字段
    public WeaponFireDelegate fireDelegate;
    private void Awake()
    {
        S = this;
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }
    void Start()
    {
        //重置武器，让主角飞船从1个高爆式武器开始
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }
    // Update is called once per frame
    void Update () {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        bounds.center = transform.position;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if(off!=Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }
        //让飞船旋转角度，更具有动感
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //使用fireDelegate委托发射武器
        //首先，确认玩家按下了Axis（“Jump”）按钮
        //然后确认fireDelegate不为null，避免产生错误
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
            fireDelegate();
	}
    public GameObject lastTriggerGo = null;
    private void OnTriggerEnter(Collider other)
    {
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        if (go != null)
        {
            if (go == lastTriggerGo)
                return;
            lastTriggerGo = go;
            if (go.tag == "Enemy")
            {
                shieldLevel--;
                Destroy(go);
            }
            else if (go.tag == "PowerUp")
            {
                //如果护盾被升级道具触发
                AbsorbPowerUp(go.transform.parent.gameObject);
            }
            else
            {
                print("触发碰撞事件：" + other.gameObject.name);
            }
        }
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            //value有问题！
            //shieldLevel = Mathf.Min(_shieldLevel, 4);
            if(_shieldLevel<0)
            {
                Main.S.DelayedRestart(gameRestartDelay);
                Destroy(this.gameObject);
            }
        }
    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch(pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                //检查当前武器类型
                if (pu.type == weapons[0].type)
                {
                    //增加该类型武器的数量
                    Weapon w = GetEmptyWeaponSlot();
                    //找到一个空白武器位置
                    if (w != null)
                    {
                        //将其赋给pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    //如果武器不一样
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                    break;
                }
                pu.AbsorbedBy(this.gameObject);
        }
        Weapon GetEmptyWeaponSlot()
        {
            for(int i=0;i<weapons.Length;i++)
            {
                if(weapons[i].type==WeaponType.none)
                {
                    return (weapons[i]);
                }
            }
            return (null);
        }
        void ClearWeapons()
        {
            foreach(Weapon w in weapons)
            {
                w.SetType(WeaponType.none);
            }
        }
    }
