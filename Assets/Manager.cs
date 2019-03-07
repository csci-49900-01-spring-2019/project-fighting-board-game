using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Player> players;
    public bool gameOver;
    public int activePlayer;
    public int turnCount;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Entered start.");
        gameOver = false;
        turnCount = 1;
        activePlayer = 0;
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HighlightTiles()
    {

    }

    void ChangePlayer()
    {
        activePlayer = (activePlayer+1) % (players.Count);
        if (activePlayer == 0) turnCount++;
        Debug.Log(activePlayer);
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    void PlayerMove(int numSpaces)
    {
        players[activePlayer].StartCoroutine("Move", numSpaces);
    }
}
