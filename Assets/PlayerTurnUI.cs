using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerTurnUI : MonoBehaviour
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
        int currentPlayer = theManager.activePlayer + 1;
        GetComponent<Text>().text = "Current Player's Move: " + currentPlayer;
    }
}