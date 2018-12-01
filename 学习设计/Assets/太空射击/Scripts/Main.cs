using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour {
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemySpawnPadding = 1.5f;                                 //敌机位置间隔
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster,WeaponType.blaster,
        WeaponType.spread,
        WeaponType.shield
    };
    public bool ___________;
    public WeaponType[] activeWeaponTypes;
    public float enemySpawnRate;                                           //敌机生成时间间隔
    private void Awake()
    {
        S = this;
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        enemySpawnRate = 1f / enemySpawnPerSecond;
        Invoke("SpawnEnemy", enemySpawnRate);
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            W_DEFS[def.type] = def;
        }
    }
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //检查值是否存在于字典内
        //检索不存在的值将会抛出错误
        //下面if语句很重要
        if (W_DEFS.ContainsKey(wt))
            return (W_DEFS[wt]);
        //这会返回一个一个武器的空定义
        //这意味着它没有找到武器定义
        return (new WeaponDefinition());
    }
    private void Start()
    {
        
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for(int i=0;i<weaponDefinitions.Length;i++)
        {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }
    }
    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + enemySpawnPadding;
        float xMax = Utils.camBounds.max.x - enemySpawnPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.camBounds.max.y + enemySpawnPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", enemySpawnRate);
    }
    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void ShipDestroyed(Enemy e)
    {
        //掉落升级道具的概率
        if (Random.value <= e.powerUpDropChance)
        {
            //Random.value生成一个0-1的数字（不包括1）
            //如果e.powerUpDropChance的值为0.5f,则有50%概率
            //产生升级道具。在测试中，这个值为1f
            //选择要挑选哪个升级道具
            //从powerUpFrequence中选取其中一个可能
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            //生成升级道具
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            //将其设置为正确的武器类型
            pu.SetType(puType);
            //将其摆放在被敌机被消灭的位置
            pu.transform.position = e.transform.position;
        }
    }
}
