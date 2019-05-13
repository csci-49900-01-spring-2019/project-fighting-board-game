using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindObjectOfType<Manager>();
    }
    Manager theManager;
    // Update is called once per frame
    void Update()
    {
        int currentPlayer = theManager.activePlayer;
        string health_string = "Current health = " + theManager.players[currentPlayer].health;
        string status_string = "\nCurrent status = " + theManager.players[currentPlayer].status;
        string currency_string = "\nCurrency: " + theManager.players[currentPlayer].rubies;
        GetComponent<Text>().text = health_string + status_string + currency_string;
    }
}
