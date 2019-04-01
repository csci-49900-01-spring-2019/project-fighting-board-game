using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Player> players;
    public List<Camera> cameras;
    public Weapon_List listOfWeapons;
    public bool gameOver;
    public bool showLog;
    public int activePlayer;
    public int activeCamera;
    public int turnCount;
    public Combat combatSystem;
    public string statText;
    public string damText1;
    public string damText2;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Entered start.");
        gameOver = false;
        turnCount = 1;
        activePlayer = 0;
        activeCamera = 7;
        cameras[activeCamera].enabled = true;
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    // Update is called once per frame
    void Update()
    {
        //CameraAdjust();
    }

    public IEnumerator ShowMovementOptions(int numSpaces)
    {
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

        if (forwardTile.TileAvailable()) // if forward tile is till available (player has selected back tile)
        {
            players[activePlayer].current_tile = backTile;
            TileEffect();
        }
        else // if player has selected forward tile
        {
            players[activePlayer].current_tile = forwardTile;
            TileEffect();
        }
        forwardTile.DeactivateOutline();
        backTile.DeactivateOutline();
    }

    void TileEffect()
    {
        if (players[activePlayer].current_tile.tile_type == TileType.weapon)
        {
            int n = Random.Range(0, 25);
            Weapon draw = listOfWeapons.wepList[n];

            //players[activePlayer].inventory.Add(draw);
            players[activePlayer].currentWeapon = draw;
            Debug.Log("You have landed on Weapons tile!" + " You drew a " + draw.finalName);

        }
        else if (players[activePlayer].current_tile.tile_type == TileType.heal)
        {
            int n = Random.Range(10, 41);
            players[activePlayer].health = players[activePlayer].health + n;
            if (players[activePlayer].health > 100)
                players[activePlayer].health = 100;
            Debug.Log("You have landed on a Healing tile!" + " You have been healed up to " + (players[activePlayer].health + n) +" health!");
        }
        else if (players[activePlayer].current_tile.tile_type == TileType.ruby)
        {
            int n = Random.Range(10, 41);
            players[activePlayer].rubies = players[activePlayer].rubies + n;
            Debug.Log("You have landed on a ruby mine!" + " You have mined " + n + " rubies!");
        }
    }

    void ChangePlayer()
    {
        CheckGameStatus();
        if (!gameOver)
        { 
            int newPlayer = (activePlayer + 1) % (players.Count);
            Debug.Log(newPlayer);
            activePlayer = newPlayer;
            CameraAdjust();
            players[activePlayer].StartCoroutine("TakeTurn");
        }
    }

    void CheckGameStatus()
    {
        int livePlayerCount = 0;
        Player livePlayer = players[activePlayer];
        for (int n = 0; n<players.Count; n++)
        {
            if (players[n].status != State.dead)
            {
                ++livePlayerCount;
                livePlayer = players[n];
            }
        }
        if (livePlayerCount == 0)
        {
            gameOver = true;
            EndGameTie();
        }
        else if (livePlayerCount == 1)
        {
            gameOver = true;
            EndGameWin(livePlayer);
        }
    }

    void EndGameTie()
    {
        Debug.Log("The game is over!");
        Debug.Log("Everyone's dead, nobody wins! :");
        Debug.Log("Total number of turns taken: " + turnCount);
    }

    void EndGameWin(Player winner)
    {
        Debug.Log("The game is over!");
        Debug.Log(winner.playerName + " has won!");
        Debug.Log("Total number of turns taken: " + turnCount);
    }

    void CameraAdjust()
    {
        float minDist = 100.0f;
        int bestCam = activeCamera;
        for (int c = 0; c < cameras.Count; ++c)
        {
            if (Vector3.Distance(cameras[c].transform.position, players[activePlayer].transform.position) < minDist)
            {
                minDist = Vector3.Distance(cameras[c].transform.position, players[activePlayer].transform.position);
                bestCam = c;
            }
        }
        cameras[activeCamera].enabled = false;
        activeCamera = bestCam;
        cameras[activeCamera].enabled = true;
    }

    void PlayerSelectSpace(int numSpaces)
    {
        StartCoroutine("ShowMovementOptions",numSpaces);
    }

    void HandleCollision(int current_player)
    {
        for (int n = 0; n < players.Count; n++)
        {
            if (n != current_player && players[n].current_tile == players[current_player].current_tile)
            {
                players[n].AdjustPosition();
                players[current_player].AdjustPosition();
                startCombat(players[current_player], players[n]);
            }
        }
    }

    void PlayerMove(Vector3 position)
    {
        players[activePlayer].StartCoroutine("Move", position);
    }

    public bool checkRangeSelf(Player P1, Player P2)    //checks range from user to enemy based off weapon range
    {
        GameTile test = P1.current_tile;    //following section checks forward range
        for (int i = 0; i <= P1.currentWeapon.range; i++)
        {
            if (test == P2.current_tile)
                return true;
            else
            {
                test = test.next_tile;
            }
        }

        test = P1.current_tile;             //following section checks backward range
        for (int i = 0; i <= P1.currentWeapon.range; i++)
        {
            if (test == P2.current_tile)
                return true;
            else
            {
                test = test.prev_tile;
            }
        }

        return false;
    }

    public bool checkRangeForEnemy(Player P2, Player P1)    //checks range from enemy to user, used for after combat is initiated
    {
        GameTile test = P2.current_tile;    //following section checks forward range
        for (int i = 0; i <= P2.currentWeapon.range; i++)
        {
            if (test == P1.current_tile)
                return true;
            else
            {
                test = test.next_tile;
            }
        }

        test = P2.current_tile;             //following section checks backward range
        for (int i = 0; i <= P2.currentWeapon.range; i++)
        {
            if (test == P1.current_tile)
                return true;
            else
            {
                test = test.prev_tile;
            }
        }

        return false;
    }

    public bool inflictStatus(Player P1, Player P2) //P1 is target player and P2 is attacking player
    {
        if (P1.status == State.normal)
        {
            State effect = P2.currentWeapon.statusEffect;

            switch (effect)
            {
                case State.normal:
                    return false;
                case State.burned:
                    int perc = Random.Range(1, 10);
                    if (perc > 3)
                        return false;
                    else
                    {
                        P1.status = effect;
                        statText = P2.playerName + " has burned " + P1.playerName;
                        return true;
                    }
                case State.poisoned:
                    perc = Random.Range(1, 10);
                    if (perc > 4)
                        return false;
                    else
                    {
                        P1.status = effect;
                        statText = P2.playerName + " has poisoned " + P1.playerName;
                        return true;
                    }
                case State.stunned:
                    perc = Random.Range(1, 10);
                    if (perc > 1)
                        return false;
                    else
                    {
                        P1.status = effect;
                        statText = P2.playerName + " has stunned " + P1.playerName + "; lucky";
                        return true;
                    }
                case State.dead:
                    statText = P2.playerName + "'s target is dead, hasn't " + P1.playerName + " suffered enough?";
                    return false;
            }
        }
        statText = P1.playerName + " already has a status condition";
        return false;
    }

        public void startCombat(Player P1, Player P2)    //this should be called after checking for range from the user to an enemy, hence P1's range is definitely in range
    // P1 is the player who INITIATES the attack.
    {

        int damage1 = P1.currentWeapon.Hit();
        int damage2 = 0;

        inflictStatus(P2, P1);
        if (checkRangeForEnemy(P2, P1))
        {
            damage2 = P2.currentWeapon.Hit();
            inflictStatus(P1, P2);
        }

        //modify damage1 and damage2 based on current tiles or otherwise
        if (P1.current_tile.tile_type == TileType.attack)
        {
            damage1 += (int)(damage1 * 0.05 + 0.5); // adding 0.5 ensures number is rounded up, if necesarry
        }
        else if (P1.current_tile.tile_type == TileType.defense)
        {
            damage2 -= (int)(damage2 * 0.05 + 0.5); // adding 0.5 ensures number is rounded up, if necesarry
        }
        if (P2.current_tile.tile_type == TileType.attack)
        {
            damage2 += (int)(damage2 * 0.05 + 0.5); // adding 0.5 ensures number is rounded up, if necesarry
        }
        else if (P2.current_tile.tile_type == TileType.defense)
        {
            damage1 -= (int)(damage1 * 0.05 + 0.5); // adding 0.5 ensures number is rounded up, if necesarry
        }
        damText1 = P1.playerName + " dealt " + damage1 + " to " + P2.playerName;
        P2.health = P2.health - damage1;
        if (P2.health > 100)
            P2.health = 100;
        damText2 = P2.playerName + " dealt " + damage2 + " to " + P2.playerName;
        P1.health = P1.health - damage2;
        if (P1.health > 100)
            P1.health = 100;
    }



}
