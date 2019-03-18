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

    public IEnumerator ShowMovementOptions(int numSpaces)
    {
        Debug.Log("Entered ShowMove...");
        GameTile forwardTile = players[activePlayer].current_tile.next_tile;
        GameTile backTile = players[activePlayer].current_tile.prev_tile;
        for (int x = 1; x < numSpaces; ++x)
        {
            forwardTile = forwardTile.next_tile;
            backTile = backTile.prev_tile;
        }
        forwardTile.ActivateOutline();
        backTile.ActivateOutline();

        while (forwardTile.TileAvailable() && backTile.TileAvailable())
            yield return null;

        if (forwardTile.TileAvailable())
        {
            players[activePlayer].current_tile = backTile;
        }
        else
        {
            players[activePlayer].current_tile = forwardTile;
        }
        forwardTile.DeactivateOutline();
        backTile.DeactivateOutline();
    }

    void ChangePlayer()
    {
        activePlayer = (activePlayer+1) % (players.Count);
        if (activePlayer == 0) turnCount++;
        Debug.Log(activePlayer);
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    void PlayerSelectSpace(int numSpaces)
    {
        StartCoroutine("ShowMovementOptions",numSpaces);
    }

    void HandleCollision(int index)
    {
        for (int n = 0; n < players.Count; n++)
        {
            if (n != index && players[n].current_tile == players[index].current_tile)
            {
                players[n].AdjustPosition();
                players[index].AdjustPosition();
                //startCombat(players[index], players[n])
            }
        }
    }

    void PlayerMove(Vector3 position)
    {
        players[activePlayer].StartCoroutine("Move", position);
    }


}
