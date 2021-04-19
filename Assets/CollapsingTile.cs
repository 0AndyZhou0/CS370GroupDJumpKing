using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollapsingTile : MonoBehaviour
{
    Queue collapseTimes = new Queue();
    Queue respawnTimes = new Queue();
    TileBase tb;

    public GameObject player;

    Tilemap collapsingTileset;

    bool onPlatform = false;

    void Start()
    {
        collapsingTileset = this.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if(onPlatform == true){
            Vector3Int tilePosition = collapsingTileset.WorldToCell(player.transform.position + Vector3.down);
            //Debug.Log(tilePosition);
            collapseTimes.Enqueue(Time.time + 3f);
            collapseTimes.Enqueue(tilePosition);
            //collapsingTileset.SetTile(tilePosition, null);
            tb = collapsingTileset.GetTile(collapsingTileset.WorldToCell(tilePosition));
            //Debug.Log(collapsingTileset.GetTile(collapsingTileset.WorldToCell(tilePosition)));
        }
        if(collapseTimes.Count != 0 && (float)collapseTimes.Peek() <= Time.time)
        {
            collapseTimes.Dequeue();
            Vector3Int tilePosition = (Vector3Int)collapseTimes.Dequeue();
            //Debug.Log(tilePosition);
            respawnTimes.Enqueue(Time.time + 10f);
            respawnTimes.Enqueue(tilePosition);
            collapsingTileset.SetTile(tilePosition, null);
        }

        if(respawnTimes.Count != 0 && (float)respawnTimes.Peek() <= Time.time)
        {
            respawnTimes.Dequeue();
            Vector3Int tilePosition = (Vector3Int)respawnTimes.Dequeue();
            collapsingTileset.SetTile(tilePosition, tb);
        }
    }

    
    //Math

    bool Approximately(float a, float b, float e) 
    {
        //Debug.Log(Mathf.Abs(a - b));
        return Mathf.Abs(a - b) < e;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        onPlatform = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        onPlatform = false;
    }
}
