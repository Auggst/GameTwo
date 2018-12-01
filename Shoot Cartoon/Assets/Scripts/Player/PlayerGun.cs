using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    public enum WeaponType
    {
        none,   //默认
        spread,  //双发
        fire,  //火焰
        ice    //寒冰
    }
    [System.Serializable]
    public class WeaponDefinition
    {
        public WeaponType type = WeaponType.none;
        public string letter;
        public Color color = Color.white;
        public ParticleSystem projectiilePrefab;
        public float damageOnHit = 0;
        public float continuousDamage = 0;
        public float delayBetweenShots = 0;
    }
}
