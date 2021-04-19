using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public static bool practiceMode = false;
    public static bool cheatMode = false;

    public void lvlSelect()
    {
        SceneManager.LoadScene("WholeGame");
    }

    public void settings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void characterSelect()
    {
        SceneManager.LoadScene("CharacterSelectScreen");
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("TitleScreenBasic");
    }

    public void PracticeModeOn()
    {
        practiceMode = !practiceMode;
    }

    public void CheatsOn()
    {
        cheatMode = !cheatMode;
    }

}
