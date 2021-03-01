using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        Vector3 setPosition = transform.position;
        setPosition.x = player.transform.position.x;
        setPosition.y = player.transform.position.y;
        transform.position = setPosition;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 setPosition = transform.position;
        //Debug.Log(setPosition);
        //Debug.Log(player.transform.position);
        setPosition.y = player.transform.position.y + 5;
        transform.position = setPosition;
    }
}
