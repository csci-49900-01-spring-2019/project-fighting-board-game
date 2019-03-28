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
        GetComponent<Text>().text = "Current health = " + theManager.players[currentPlayer].health;
    }
}
