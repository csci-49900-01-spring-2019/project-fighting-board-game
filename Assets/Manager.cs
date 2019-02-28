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
        gameOver = false;
        activePlayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        players[activePlayer].TakeTurn();
    }

    void ChangePlayer()
    {
        activePlayer = (activePlayer+1) % (players.Count);
    }

    void PlayerMove(int numSpaces)
    {
        players[activePlayer].StartCoroutine("Move");
    }
}
