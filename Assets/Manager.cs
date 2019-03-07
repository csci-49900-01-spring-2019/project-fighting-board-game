using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Player> players;
    public bool gameOver;
    public int activePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Entered start.");
        gameOver = false;
        activePlayer = 0;
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangePlayer()
    {
        Debug.Log(activePlayer);
        Debug.Log(players.Count);
        activePlayer = (activePlayer+1) % (players.Count);
        Debug.Log(activePlayer);
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    void PlayerMove(int numSpaces)
    {
        players[activePlayer].StartCoroutine("Move", numSpaces);
    }
}
