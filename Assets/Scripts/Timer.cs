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

        string minutes = ((int) timer / 60).ToString();
        string seconds = (timer % 60).ToString("f2");


        GetComponent<TMPro.TextMeshProUGUI>().text = minutes + seconds;
    }
}
