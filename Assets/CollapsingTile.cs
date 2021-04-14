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

    void OnCollisionEnter2D(Collision2D col){
        Tilemap collapsingTileset = this.GetComponent<Tilemap>();
        foreach (ContactPoint2D tile in col.contacts)
        {
            Vector2 tilePosition = new Vector2();
            tilePosition.x = tile.point.x - 0.01f * tile.normal.x;
            tilePosition.y = tile.point.y - 0.01f * tile.normal.y;
            collapsingTileset.SetTile(collapsingTileset.WorldToCell(tilePosition), null);
        }
    }
}
