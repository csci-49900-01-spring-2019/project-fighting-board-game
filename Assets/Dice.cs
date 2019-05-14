using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generate a random number between two numbers

public class Dice : MonoBehaviour
{
    public int range_start;
    public int range_end;
    public bool roll_processed;
    public int roll;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        range_start = 1;
        range_end = 6;
        roll_processed = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public int GetRoll()
    {
        if (!roll_processed)
        {
            roll_processed = true;
            return last_roll;
        }
        else
        {
            Debug.Log("You didn't roll, silly!");
            return 0;
        }
    }
    */

    private void OnMouseDown()
    {
        Debug.Log("Active Player: " + manager.activePlayer);
        if (PhotonNetwork.LocalPlayer.ActorNumber != -1)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != manager.activePlayer + 1)
            {
                Debug.Log("Not your turn to move!");
                Debug.Log("Local Actor #: " + PhotonNetwork.LocalPlayer.ActorNumber + " ActiveP = " + (manager.activePlayer + 1));
                return;
            }
            manager.players[manager.activePlayer].isActive = true;
        }
       

        if (roll_processed)
        {
            roll = Random.Range(range_start, range_end);
            Debug.Log("Dice rolled, you got " + roll);
            gameObject.SendMessageUpwards("PlayerSelectSpace", roll);
            roll_processed = false;

        }
        else
        {
            Debug.Log("Hey, you already rolled!");
        }
    }

    public void MakeDieAvailable()
    {
        roll_processed = true;
    }
}
