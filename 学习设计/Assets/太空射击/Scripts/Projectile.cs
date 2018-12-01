using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private WeaponType _type;                   
    //这个全局属性屏蔽了_type字段，并在设置该字段时运行一段代码
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
    private void Awake()
    {
        //每两秒检测一次，查看本对是否出了屏幕范围
        InvokeRepeating("CheckOffscreen", 2f, 2f);
    }
    public void SetType(WeaponType eType)
    {
        //设置_type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        GetComponent<Renderer>().material.color = def.projectileColor;
    }
    void CheckOffscreen()
    {
        if(Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds,BoundsTest.offScreen)!=Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}
