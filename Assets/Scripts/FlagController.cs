using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    public static void FixedUpdate()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1]-(float)0.15;
        position.z = data.position[2]+1;
        GameObject.FindWithTag("CheckpointFlag").transform.position = position;
    }
}
