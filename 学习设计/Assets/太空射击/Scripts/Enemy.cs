using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;                     //击毁敌机获得点数
    public int showDamageFrames = 2;            //显示伤害效果的帧数
    public float powerUpDropChance = 1f;        //掉落升级道具的概率
    public bool ________;
    public Color[] originalColors;
    public Material[] materials;                //本对象及其子对象的所有材质
    public int remainingDamageFrames = 0;       //剩余的伤害效果帧数
    public Bounds bounds;                       //本对象及其子对象的边界框
    public Vector3 boundsCenterOffset;
    private void Awake()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i=0;i<materials.Length;i++)
        {
            originalColors[i] = materials[i].color;
        }
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }
    // Update is called once per frame
    void Update () {
        Move();
        if(remainingDamageFrames>0)
        {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0)
                UnShowDamage();
        }
	}
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    void CheckOffscreen()
    {
        if(bounds.size==Vector3.zero)
        {
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }
        bounds.center = transform.position + boundsCenterOffset;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero)
            if (off.y < 0)
                Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //进入屏幕前，敌机不会受到伤害
                //这可避免玩家射击到屏幕外看不到的敌机
                bounds.center = transform.position + boundsCenterOffset;
                if(bounds.extents==Vector3.zero||Utils.ScreenBoundsCheck(bounds,BoundsTest.offScreen)!=Vector3.zero)
                {
                    Destroy(other);
                    break;
                }
                ShowDamage();
                health -= Main.W_DEFS[p.type].damageOnHit;
                if(health<=0)
                {
                    //通知Main单例对象敌机已经被消灭
                    Main.S.ShipDestroyed(this);
                    //消灭敌机
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }
    void ShowDamage()
    {
        foreach(Material m in materials)
        {
            m.color = Color.red;
        }
        remainingDamageFrames = showDamageFrames;
    }
    void UnShowDamage()
    {
        for(int i=0;i<materials.Length;i++)
        {
            materials[i].color = originalColors[i];
        }
    }
}
