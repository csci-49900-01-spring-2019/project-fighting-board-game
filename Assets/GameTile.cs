using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTile : MonoBehaviour
{
    public GameTile next_tile;
    public GameTile prev_tile;
    public bool available;


    // Start is called before the first frame update
    void Start()
    {
        Outline outline = GetComponent<Outline>();
        outline.enabled = false;
        available = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnArrivial(Player p)
    {
        //do nothing
    }

    public void ActivateOutline()
    {
        Outline outline = GetComponent<Outline>();
        outline.enabled = true;
        available = true;
    }

    public void DeactivateOutline()
    {
        Outline outline = GetComponent<Outline>();
        outline.enabled = false;
        available = false;
    }

    public bool TileAvailable()
    {
        return available;
    }

    private void OnMouseDown()
    {
        Outline outline = GetComponent<Outline>();
        if (outline.enabled == true)
        {
            Vector3 pos = GetTilePosition();
            gameObject.SendMessageUpwards("PlayerMove", pos);
        }
        available = false;
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
