using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighScore : MonoBehaviour {
    static public int score = 1000;
    // Use this for initialization
    private void Awake()
    {
        if(PlayerPrefs.HasKey("ApplePickerHightScore"))
        {
            score = PlayerPrefs.GetInt("ApplePickerHighScore");
        }
        PlayerPrefs.SetInt("AppllePickerHighScore", score);
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Text gt = this.GetComponent<Text>();
        gt.text = "历史最高：" + score;
        if(score>PlayerPrefs.GetInt("ApplePickerHighScore"))
        {
            PlayerPrefs.SetInt("ApplePickerHighScore", score);
        }
	}
}
