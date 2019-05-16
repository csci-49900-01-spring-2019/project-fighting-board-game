using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory1Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<Player>();
    }
    Player thePlayer;

    public void selectWeapon()
    {
        if (thePlayer.itemSwap == false)
        {
            thePlayer.currentWeapon = thePlayer.inventory[0];
            Debug.Log("Your current weapon is " + thePlayer.currentWeapon);
        }
    }
}