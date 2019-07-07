using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a super simple script that counts the time backwards until it reaches zero, once it does, it shows you a message saying game over
public class GameOver : MonoBehaviour {
    [SerializeField] float totalTime;
    [SerializeField] string msg;
    [SerializeField] float displayTime;
    private bool displayMsg;
    private float timeCounter;

    void Start()
    {
        timeCounter = totalTime;
    }

    void Update () {
        timeCounter -= Time.deltaTime;
        if (timeCounter <= totalTime)
        {
            totalTime--;
            Debug.Log(totalTime);
        }
        if (timeCounter <= 0)
        {
            displayTime -= Time.deltaTime;
            if (displayTime <= 0)
            {
                displayMsg = false;
            }
            else
            {
                displayMsg = true;
            }
        }
	}

    void OnGUI()
    {
        if (displayMsg)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), msg);
        }
    }
}