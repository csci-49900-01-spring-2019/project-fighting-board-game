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
    public string weapon;
    public string armor;
    //public bool isActive;
    public bool hasMoved;
    public Dice my_die;
    public GameTile current_tile;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("My name is " + playerName);
        hasMoved = false;
        health = 100;

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Random.ColorHSV());
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.black);
    }

    public void PlayerAttacked(int damage, State effect = State.normal)
    {
        health -= damage;
        if (health <= 0)
            status = State.dead;
        else
            status = effect;
    }

    public void UpdatePlayerStatus()
    {
        //Fetch the Renderer from the GameObject
        Renderer rend = GetComponent<Renderer>();

        switch (status)
        {
            case State.normal:
                //Find the Specular shader and change its Color to black
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.black);
                break;
            case State.stunned:
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.yellow);
                break;
            case State.poisoned:
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.green);
                health -= 1;
                break;
            case State.burned:
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.red);
                health -= 1;
                break;
            default: // status == dead
                rend.material.shader = Shader.Find("Specular");
                rend.material.SetColor("_SpecColor", Color.white);
                break;
        }
    }

    public IEnumerator Move(int numSpaces)
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
        yield return null;
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

    public IEnumerator TakeTurn()
    {

        Debug.Log("Entered TakeTurn.");
        hasMoved = false;
        while (!hasMoved)
            yield return null;
        UpdatePlayerStatus();
        EndTurn();
    }

    public void EndTurn()
    {
        Debug.Log("Turn has ended!");
        gameObject.SendMessageUpwards("ChangePlayer");
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
