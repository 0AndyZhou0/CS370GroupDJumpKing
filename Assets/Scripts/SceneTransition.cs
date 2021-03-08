using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = null;

    public void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
