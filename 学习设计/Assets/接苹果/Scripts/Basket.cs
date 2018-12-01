using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Basket : MonoBehaviour {
    public Text scoreGT;
    public int score;
    private int _hardLevel;
    // Use this for initialization
    void Start()
    {
        GameObject scoreGo = GameObject.Find("ScoreCounter");
        scoreGT = scoreGo.GetComponent<Text>();
        scoreGT.text = "得分:0";
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
        _hardLevel = score / 1000;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collideWith = collision.gameObject;
        if (collideWith.tag == "Apple")
            Destroy(collideWith);
        score += 100;
        scoreGT.text = "得分：" + score.ToString();
        switch (_hardLevel)
        {
            case 1:
                AppleTree._Speed = 2f;
                break;
            case 2:
                AppleTree._Speed = 4f;
                break;
            case 3:
                AppleTree._Speed = 8f;
                break;
            case 4:
                AppleTree._Speed = 16f;
                break;
        }
    }
}

