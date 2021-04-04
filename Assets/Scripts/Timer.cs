using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer = 0.0f;

    void Start()
    {
        if(PlayerPrefs.HasKey("timer")){
            timer = PlayerPrefs.GetFloat("timer");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += (Time.deltaTime / 1.7f);
        PlayerPrefs.SetFloat("timer", timer);
        //Debug.Log(timer);

        double seconds = timer;
        int minutes = (int) (seconds / 60.0);
        int hours = (int) (minutes / 60.0);
        
        seconds %= 60.0;
        minutes %= 60;

        string time = "";

        if(hours == 0){
            time += "00:";
        }else if(hours < 10){
            time += "0" + hours.ToString() + ":";
        }else{
            time += hours.ToString() + ":";
        }

        if(minutes == 0){
            time += "00:";
        }else if(minutes < 10){
            time += "0" + minutes.ToString() + ":";
        }else{
            time += minutes.ToString() + ":";
        }
        
        if(seconds == 0){
            time += "00.00";
        }else if(seconds < 10){
            time += "0" + seconds.ToString("f2");
        }else{
            time += seconds.ToString("f2");
        }

        GetComponent<TMPro.TextMeshProUGUI>().text = time;
    }
}
