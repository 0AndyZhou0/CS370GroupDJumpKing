using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollapsingTile : MonoBehaviour
{
    Queue collapseTimes = new Queue();
    Queue respawnTimes = new Queue();
    TileBase tb;

    Tilemap collapsingTileset;

    void Start()
    {
        collapsingTileset = this.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if(collapseTimes.Count != 0 && (float)collapseTimes.Peek() <= Time.time)
        {
            collapseTimes.Dequeue();
            Vector2 tilePosition = (Vector2)collapseTimes.Dequeue();
            respawnTimes.Enqueue(Time.time + 10f);
            respawnTimes.Enqueue(tilePosition);
            collapsingTileset.SetTile(collapsingTileset.WorldToCell(tilePosition), null);
        }

        if(respawnTimes.Count != 0 && (float)respawnTimes.Peek() <= Time.time)
        {
            respawnTimes.Dequeue();
            Vector2 tilePosition = (Vector2)respawnTimes.Dequeue();
            collapsingTileset.SetTile(collapsingTileset.WorldToCell(tilePosition), tb);
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
        foreach (ContactPoint2D tile in col.contacts)
        {
            if(Approximately(tile.normal.x, 0.0f, 0.01f) && Approximately(tile.normal.y, -1.0f, 0.01f))
            {
                Vector2 tilePosition = new Vector2();
                tilePosition.x = tile.point.x + 0.01f * tile.normal.x;
                tilePosition.y = tile.point.y + 0.01f * tile.normal.y;
                collapseTimes.Enqueue(Time.time + 3f);
                collapseTimes.Enqueue(tilePosition);
                //collapsingTileset.SetTile(collapsingTileset.WorldToCell(tilePosition), null);
                tb = collapsingTileset.GetTile(collapsingTileset.WorldToCell(tilePosition));
                Debug.Log(collapsingTileset.GetTile(collapsingTileset.WorldToCell(tilePosition)));
            }
        }
    }
}
