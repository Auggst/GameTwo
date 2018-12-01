using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MianCamera : MonoBehaviour {
    public GameObject Baskets;
    public int numBaskets = 3;
    public float BasketBottonmY = -14f;
    public float BasketSpacingY = 2f;
    public List<GameObject> basketList;
    // Use this for initialization
    void Start()
    {

        basketList = new List<GameObject>();
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGo = Instantiate(Baskets) as GameObject;
            Vector3 pos = Vector3.zero;
            pos.y = BasketBottonmY + (BasketSpacingY * i);
            tBasketGo.transform.position = pos;
            basketList.Add(tBasketGo);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
