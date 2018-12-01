using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none=0,   //默认
    spread=1,  //双发
    fire=2,  //火焰
    ice=3    //寒冰
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
public class PlayerShooting : MonoBehaviour {
    public static PlayerShooting S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject fireGun;
    public GameObject iceGun;
    public float speed=200;
    public bool __________________;

    public WeaponType[] activeWeaponTypes;
    public WeaponType nowWeapon;
    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;

    float effectsDisplayTime = 0.2f;

    private void Awake()
    {
        S = this;
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
            W_DEFS[def.type] = def;
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++)
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        gunParticles = GetComponent<ParticleSystem>();
        nowWeapon = activeWeaponTypes[2];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") )
        {
            switch (nowWeapon) {
                case WeaponType.none:
                    timeBetweenBullets = .15f;
                    if (timer >= timeBetweenBullets)
                        Shoot();
                    StartCoroutine(CloseEffects());
                    break;
                case WeaponType.spread:
                    timeBetweenBullets = 1;
                    Shoot();
                    StartCoroutine(CloseEffects());
                    break;
                case WeaponType.fire:
                    timeBetweenBullets = .15f;
                    if (timer >= timeBetweenBullets)
                        Fire();
                    //StartCoroutine(CloseEffects());
                    break;
                case WeaponType.ice:
                    Ice();
                    break;
                default:
                    Shoot();
                    break;
            }
        }

    }
    //停用
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
    IEnumerator CloseEffects()
    {
        yield return new WaitForEndOfFrame();
        DisableEffects();
    }
    //射击
    void Shoot()
    {
        timer = 0f;
        gunLight.enabled = true;
        gunParticles.Stop();
        gunParticles.Play();
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

    }
    void Fire()
    {
        timer = 0f;
        gunLight.enabled = true;
        Vector3 point = shootHit.point;
        Transform transform = this.gameObject.transform;
        transform.position = shootHit.point;
        Instantiate(fireGun, transform);
        GameObject.FindGameObjectWithTag("Fire_Gun").GetComponent<Rigidbody>().AddForce(transform.forward * speed);

    }
    void Ice()
    {
    }
}

