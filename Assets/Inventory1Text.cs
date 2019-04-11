using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory1Text : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindObjectOfType<Manager>();
        theDice = GameObject.FindObjectOfType<Dice>();
    }
    Manager theManager;
    Dice theDice;
    // Update is called once per frame
    void Update()
    {
        int currentPlayer = theManager.activePlayer;
        if (theManager.players[currentPlayer].inventory.Count > 1)
        {
            if (theDice.roll_processed == false)
            {
                GetComponent<Text>().text = theManager.players[currentPlayer].inventory[1].Wname;
            }
        }
    }
}

