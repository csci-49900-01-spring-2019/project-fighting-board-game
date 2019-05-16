using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileType { normal, attack, defense, weapon, heal, ruby, trap };

public class GameTile : MonoBehaviour
{
    public GameTile next_tile;
    public GameTile prev_tile;
    public bool available;
    public TileType tile_type;
    public bool has_store = false;

    // Start is called before the first frame update
    void Start()
    {
        int select_type = Random.Range(0, 100);
        if (select_type < 5)
            tile_type = TileType.normal;
        else if (select_type < 15)
            tile_type = TileType.trap;
        else if (select_type < 50)
            tile_type = TileType.weapon;
        else if (select_type < 60)
            tile_type = TileType.attack;
        else if (select_type < 70)
            tile_type = TileType.defense;
        else if (select_type < 85)
            tile_type = TileType.ruby;
        else
            tile_type = TileType.heal;
        Outline outline = GetComponent<Outline>();
        outline.enabled = false;
        available = false;
        InitializeTile();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeTile()
    {
        Renderer rend = GetComponent<Renderer>();
        switch (tile_type)
        {
            case TileType.attack:
                //Find the Specular shader and change its Color to black
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.red);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            case TileType.defense:
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.gray);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            case TileType.weapon:
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.yellow);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            case TileType.ruby:
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.magenta);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            case TileType.heal:
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.blue);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            case TileType.trap:
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.green);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            default:
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.white);
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
        }
    }

    //public void OnArrivial(Player p)
    //{
    //if (tile_type == "attack")
    //   p
    //}

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
