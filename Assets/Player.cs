using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLAYER CLASS SCRIPT

public class Player : MonoBehaviour
{
    public enum State { normal, burned, poisoned, stunned, dead };
    public string playerName;
    public State status;
    public int health;
    public int money;
    public string currentWeapon;
    public List<string> weapons;
    public string armor;
    public bool isActive;
    public Dice my_die;
    public GameTile current_tile;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I am alive and my name is " + playerName);

        //Fetch the Renderer from the GameObject
        Renderer rend = GetComponent<Renderer>();

        switch (status)
        {
            case State.normal:
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.white);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);
                break;
            case State.stunned:
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.yellow);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);
                break;
            case State.poisoned:
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.green);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);
                break;
            case State.burned:
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.red);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);
                break;
            default: // status == dead
                //Set the main Color of the Material to green
                rend.material.shader = Shader.Find("_Color");
                rend.material.SetColor("_Color", Color.gray);

                //Find the Specular shader and change its Color to red
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);
                break;
        }
       
    }

    IEnumerator Move()
    {
        int dieRoll = my_die.GetRoll();
        for (int x = 0; x < dieRoll; ++x)
        { 
            Vector3 nextTile = current_tile.GetNextTilePosition();
            float n = 0.0f;
            Vector3 currentPosition = current_tile.GetTilePosition();
            Vector3 nextPosition = current_tile.GetNextTilePosition();
            //Debug.Log(currentPosition);
            //Debug.Log(nextPosition);
            while (n < 1f)
            {
                Transform tf = GetComponent<Transform>();
                float journeyLength = Vector3.Distance(currentPosition, nextPosition);
                tf.position = Vector3.Lerp(currentPosition, nextPosition, n);
                n += 0.1f;
                yield return null;
            }
            current_tile = current_tile.next_tile;
        }
    }

    private void FixedUpdate()
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        StartCoroutine("Move");
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "End Turn"))
        {
            print("Your turn is now over.");
            isActive = false;
        }
    }
}
