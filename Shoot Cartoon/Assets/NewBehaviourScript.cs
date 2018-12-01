using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public GameObject newObject;
    private static int cubeCount = 0;
    public float speed = 2f;
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            if (cubeCount <= 10)
            {
                Instantiate(newObject.transform, transform.position, transform.rotation);
                GameObject.FindGameObjectWithTag("Fire_Gun").GetComponent<Rigidbody>().AddForce(transform.forward * speed);
                Debug.Log(newObject.transform.position);
                cubeCount++;
            }
        }
	}
}
