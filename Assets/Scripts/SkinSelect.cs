using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelect : MonoBehaviour
{
    public void GreyGuySelected()
    {
        PlayerPrefs.SetInt("skin", 0);
    }

    public void GhostSelected()
    {
        PlayerPrefs.SetInt("skin", 1);
    }
}
