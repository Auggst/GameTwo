using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ApplePicker : MonoBehaviour
{
    public GameObject Baskets;
    public int numBaskets = 3;
    public float BasketBottonmY = -14f;
    public float BasketSpacingY = 2f;
    public List<GameObject> basketList;
    public GameObject over;
    public static  bool isOver;
    // Use this for initialization
    void Start()
    {
        isOver = false;
        over.SetActive(false);
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

    public void AppleDestroyed()
    {
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGo in tAppleArray)
        {
            Destroy(tGo);
        }
        int basketIndex = basketList.Count - 1;
        GameObject tBacketGo = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBacketGo);
        if(basketList.Count==0)
        {
            isOver = true;
            over.SetActive(true);
        }
    }
}