using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory2Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<Player>();
        theDice = GameObject.FindObjectOfType<Dice>();
    }
    Player thePlayer;
    Dice theDice;
    public void selectWeapon()
    {
        if (thePlayer.itemSwap == false && thePlayer.inventory.Count > 2)
        {
            thePlayer.currentWeapon = thePlayer.inventory[2];
            Debug.Log("Your current weapon is " + thePlayer.currentWeapon);
        }
    }
    public void swap()
    {
        if (theDice.roll_processed == true && thePlayer.itemSwap == true)
        {
            if (!string.IsNullOrEmpty(thePlayer.extraItem))
            {
                thePlayer.inventory[2].Wname = thePlayer.extraItem;
                Debug.Log("You have swapped! You now have a " + thePlayer.inventory[2]);
                thePlayer.extraItem = "";
                thePlayer.itemSwap = false;
            }
        }
    }
}