using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTrigger : MonoBehaviour {
    GameObject player;
    public WeaponType thisObject;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(destroy());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            PlayerShooting.S.nowWeapon = thisObject;
            Destroy(this.gameObject);
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
