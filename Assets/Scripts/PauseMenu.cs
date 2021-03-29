using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PaueMenuUI;

    bool paused;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(paused){
                Resume();
            }else{
                Pause();
            }
        }
    }

    void Resume(){
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 0f;
    }

    void Pause(){
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1.7f;
    }
}
