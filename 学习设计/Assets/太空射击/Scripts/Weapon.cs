using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    none,      //默认/无武器
    blaster,   //高爆武器
    spread,    //双发武器
    phaser,  //未实现，波形武器
    missile,  //未实现，制导导弹
    laser,    //未实现，持续性武器
    shield    //护盾
}
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                             //道具上的字母
    public Color color = Color.white;                 //颜色
    public GameObject projectilePrefab;               //炮弹预设
    public Color projectileColor = Color.white;       //
    public float damageOnHit = 0;                     //伤害
    public float continuousDamage = 0;                //每秒的伤害
    public float delayBetweenShots = 0;
    public float velocity = 20;                       //炮弹速度
}
public class Weapon : MonoBehaviour {
    static public Transform PROJECTILE_ANCHOR;
    public bool ________________;
    [SerializeField]
    private WeaponType _type = WeaponType.blaster;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShot;
    private void Awake()
    {
        collar = transform.Find("Collar").gameObject;
    }
    // Use this for initialization
    void Start () {
        //调用SetType（），设置默认武器类型
        SetType(_type);
        if(PROJECTILE_ANCHOR==null)
        {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //查找父对象的fireDelegate
        GameObject parentGO = transform.parent.gameObject;
        if(parentGO.tag=="Hero")
        {
            _Hero.S.fireDelegate += Fire;
        }
	}
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }
	public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type==WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0;
    }
    public void Fire()
    {
        //如果this.gameObject处于未激活状态，返回
        if (!gameObject.activeInHierarchy) return;
        //如果距离上次发射的时间间隔不足最小间隔，则返回
        if(Time.time-lastShot<def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        switch(type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity=Vector3.up*def.velocity;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(-.2f, 0.9f, 0) * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(.2f, 0.9f, 0) * def.velocity;
                break;
        }
    }
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab) as GameObject;
        if(transform.parent.gameObject.tag=="Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return (p);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
