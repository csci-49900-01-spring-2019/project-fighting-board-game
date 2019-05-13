using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EndGame = GameObject.FindObjectOfType<Manager>();
        GameOver = GameObject.Find("GameOverCanvas");
    }
    Player EndPlayer;
    Manager EndGame;
    GameObject GameOver;
    // Update is called once per frame
    void Update()
    {
        if (EndGame.gameOver == true)
        {
            int livePlayerCount = 0;
            EndPlayer = EndGame.players[EndGame.activePlayer];
                for (int n = 0; n < EndGame.players.Count; n++)
            {
                if (EndGame.players[n].status != State.dead)
                {
                    ++livePlayerCount;
                    EndPlayer = EndGame.players[n];
                }
            }
            if (livePlayerCount == 0)
                GetComponent<Text>().text = "The game is over! Everyone's dead, nobody wins! Total number of turns taken: " + EndGame.turnCount;
            else 
                GetComponent<Text>().text = "Hail the conquering hero~! " + EndPlayer.playerName + " has won! Total number of turns taken: " + EndGame.turnCount;

            GameOver.GetComponent<Canvas>().enabled = true;
        }
    }
}