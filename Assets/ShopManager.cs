using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public GameTile current_tile1;
    public GameTile current_tile2;
    Manager theManager;
    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindObjectOfType<Manager>();
        Transform tf = GetComponent<Transform>();

        Vector3 pos = current_tile1.GetTilePosition();
        Vector3 pos2 = current_tile2.GetTilePosition();
        Quaternion rot = Quaternion.Euler(-90, 0, 0);
        Instantiate(prefab1, pos, rot);
        Instantiate(prefab2, pos2, rot);
        current_tile1.has_store = true;
        current_tile2.has_store = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}