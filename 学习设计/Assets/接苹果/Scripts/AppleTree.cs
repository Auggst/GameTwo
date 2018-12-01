using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {

    public GameObject Apples;
    public float LeftAndRightEdge = 10f;
    public  static float _Speed = 1f;
    private float _ChangeToDirections = 0.005f;
    public float secondsBetweenAppleDrops = 1f;
    // Use this for initialization
    void Start()
    {

        InvokeRepeating("DropApple", 1f, secondsBetweenAppleDrops);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ApplePicker.isOver)
        {
            Vector3 pos = transform.position;
            pos.x += _Speed * Time.deltaTime;
            transform.position = pos;
            if (pos.x < -LeftAndRightEdge)
            {
                _Speed = Mathf.Abs(_Speed);
            }
            else if (pos.x > LeftAndRightEdge)
            {
                _Speed = -Mathf.Abs(_Speed);
            }
        }

    }
    private void FixedUpdate()
    {
        if (Random.value < _ChangeToDirections)
        {
            _Speed *= -1;
        }
    }
    void DropApple()
    {
        GameObject apple = Instantiate(Apples) as GameObject;
        apple.transform.position = transform.position;
    }
}
