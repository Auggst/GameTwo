using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    public float rotationsPerSecond = 0.1f;
    public bool ____________;
    public int levelShown = 0;
	
	// Update is called once per frame
	void Update () {
        int currLevel = Mathf.FloorToInt(_Hero.S.shieldLevel);
        if(levelShown !=currLevel)
        {
            levelShown = currLevel;
            Material mat = this.GetComponent<MeshRenderer>().material;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        float rZ = (rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
	}
}
