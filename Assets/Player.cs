using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//PLAYER CLASS SCRIPT

public enum State { normal, stunned, poisoned, burned, dead };

public class Player : MonoBehaviour
{ 
    public int index;
    public string playerName;
    public State status;
    public int health;
    public int money;
    public Weapon currentWeapon;
    public List<Weapon> inventory;
    public string armor;
    public bool hasMoved;
    //public bool isActive;
    public Dice my_die;
    public GameTile current_tile;

    // Start is called before the first frame update
    void Start()
    {

        //GUI.Box(new Rect(10, 10, 150, 100),GUI
        Debug.Log("My name is " + playerName);
        hasMoved = false;
        health = 100;
        //Weapon aWeapon;
        currentWeapon = new Weapon("stick","","");

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Random.ColorHSV());
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.black);
    }

    private void SetName(string n)
    {
        Debug.Log(n);
        playerName = n;
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

    /*
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

    public IEnumerator Move(GameTile destination) // REVISED
    {
        while (current_tile != destination)
        {
            float n = 0.0f;

            Vector3 nextPosition = current_tile.GetNextTilePosition();
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
    */

    public IEnumerator Move(Vector3 destination)
    {
        float n = 0.0f;
        Vector3 currentPosition = current_tile.GetTilePosition();
        while (n < 1.1f)
        {  
            Transform tf = GetComponent<Transform>();
            //float journeyLength = Vector3.Distance(currentPosition, destination);
            tf.position = Vector3.Lerp(currentPosition, destination, n);
            n += 0.1f;
            yield return null;
        }
        my_die.MakeDieAvailable();
        hasMoved = true;
        yield return null;
    }

    /*
    public void ShowMovementOptions(int numSpaces)
    {
        Debug.Log("Entered ShowMove...");
        GameTile forwardTile = current_tile.next_tile;
        GameTile backTile = current_tile.prev_tile;
        for (int x = 1; x < numSpaces; ++x)
        {
            forwardTile = forwardTile.next_tile;
            backTile = backTile.prev_tile;
        }
        forwardTile.ActivateOutline();
        backTile.ActivateOutline();
    }
    */

    public IEnumerator TakeTurn()
    {
        Debug.Log("Entered TakeTurn.");
        //isActive = true;
        hasMoved = false;
        UpdatePlayerStatus();
        while (!hasMoved)
            yield return null;
        gameObject.SendMessageUpwards("HandleCollision", index);
        //while (!isActive)
        //    yield return null;
        EndTurn();
    }

    public void EndTurn()
    {
        Debug.Log("Turn has ended!");
        gameObject.SendMessageUpwards("ChangePlayer");
    }

    public void AdjustPosition()
    {
        Transform tf = GetComponent<Transform>();
        if (index < 2)
            tf.position = tf.position + new Vector3(0.2f, 0.0f, 0.0f);
        else
            tf.position = tf.position + new Vector3(-0.2f, 0.0f, 0.0f);
        if (index % 2 == 0)
            tf.position = tf.position + new Vector3(0.0f, 0.0f, 0.2f);
        else
            tf.position = tf.position + new Vector3(0.0f, 0.0f, -0.2f);
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
