using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    public GameTile next_tile;
    public GameTile prev_tile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetTilePosition()
    {
        return transform.position;
    }

    public Vector3 GetNextTilePosition()
    {
        return next_tile.transform.position;
    }

    public Vector3 GetPrevTilePosition()
    {
        return prev_tile.transform.position;
    }
}
