using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollapsingTile : MonoBehaviour
{
    bool isIntact = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //Math

    bool Approximately(float a, float b, float e) {
        //Debug.Log(Mathf.Abs(a - b));
        return Mathf.Abs(a - b) < e;
    }

    void OnCollisionEnter2D(Collision2D col){
        Tilemap collapsingTileset = this.GetComponent<Tilemap>();
        foreach (ContactPoint2D tile in col.contacts)
        {
            if(Approximately(tile.normal.x, 0.0f, 0.01f) && Approximately(tile.normal.y, -1.0f, 0.01f)){
                Vector2 tilePosition = new Vector2();
                tilePosition.x = tile.point.x + 0.01f * tile.normal.x;
                tilePosition.y = tile.point.y + 0.01f * tile.normal.y;
                collapsingTileset.SetTile(collapsingTileset.WorldToCell(tilePosition), null);
            }
        }
    }
}
