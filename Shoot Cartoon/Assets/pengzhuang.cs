using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pengzhuang : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("Fire_Gun").GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}
