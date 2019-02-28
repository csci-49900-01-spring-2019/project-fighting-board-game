using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLAYER CLASS SCRIPT

public class Player : MonoBehaviour
{
    public enum State { normal, stunned, poisoned, burned, dead };
    public string playerName;
    public State status;
    public int health;
    public int money;
    public List<string> weapons;
    public string armor;
    //public bool isActive;
    public bool hasMoved;
    public Dice my_die;
    public GameTile current_tile;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("My name is " + playerName);
        //isActive = false;
        hasMoved = false;

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

    IEnumerator Move(int numSpaces)
    { 
        for (int x = 0; x < numSpaces; ++x)
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
        my_die.MakeDieAvailable();
        hasMoved = true;
    }


    //IEnumerator WaitForRoll()
   // {
    //    while (false)
    //        yield return null;
    //}

    //IEnumerator WaitForKeyMove()
    //{
    //    while (!hasMoved)
    //        yield return null;
    //}

    public void TakeTurn()
    {
        while (!hasMoved) { }
        EndTurn();
    }

    public void EndTurn()
    {
        gameObject.SendMessage("ChangePlayer");
    }

    private void FixedUpdate()
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
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
            //isActive = false;
        }
    }
}
