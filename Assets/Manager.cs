using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Player> players;
    public List<Camera> cameras;
    public Weapon_List listOfWeapons;
    public bool gameOver;
    public int activePlayer;
    public int activeCamera;
    public int turnCount;
    public int wepDrawStart = 0;
    public int wepDrawEnd = 4;
    public string lastEvent;
    public bool eventFlag;
    public bool combatFlag;
    public Combat combatSystem;
    public string statText1 = "s1";
    public string statText2 = "s2";
    public string damText1 = "d1";
    public string damText2 = "d2";
    public decimal loot;


    // Start is called before the first frame update
    void Start()
    {
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

    void ReceiveEvent(string eventString)
    {
        lastEvent = eventString;
        eventFlag = true;
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
            int n = Random.Range(wepDrawStart, wepDrawEnd);
            Weapon draw = listOfWeapons.wepList[n];

            //players[activePlayer].inventory.Add(draw);
            players[activePlayer].currentWeapon = draw;
            ReceiveEvent(players[activePlayer].playerName + " landed on a weapons tile and drew a " + draw.finalName);
        }
        else if (players[activePlayer].current_tile.tile_type == TileType.heal)
        {
            int n = Random.Range(10, 41);
            players[activePlayer].health = players[activePlayer].health + n;
            if (players[activePlayer].health > 100)
                players[activePlayer].health = 100;
            ReceiveEvent(players[activePlayer].playerName + " landed on a Healing tile and has healed " + n +" health points!");
        }
        else if (players[activePlayer].current_tile.tile_type == TileType.trap)
        {
            int n = Random.Range(10, 25);
            players[activePlayer].health = players[activePlayer].health - n;
            if (players[activePlayer].health > 100)
                players[activePlayer].health = 100;
            ReceiveEvent(players[activePlayer].playerName + " landed on a Trap tile and lost " + n + " health points!");
        }
        else if (players[activePlayer].current_tile.tile_type == TileType.ruby)
        {
            int n = Random.Range(10, 41);
            players[activePlayer].rubies = players[activePlayer].rubies + (decimal)n;
            ReceiveEvent(players[activePlayer].playerName + " landed on a ruby mine and mined " + n + " rubies!");
        }
    }

    void ChangePlayer()
    {
        CheckGameStatus();
        if (!gameOver)
        { 
            int newPlayer = (activePlayer + 1) % (players.Count);
            if (newPlayer == 0)
            {
                turnCount += 1;
                if (turnCount % 5 == 0) { 
                    if (wepDrawEnd != 24)
                    {
                        wepDrawStart += 5;
                        wepDrawEnd += 5;
                    }
                }
            }
            ReceiveEvent(players[newPlayer].playerName + "'s turn has started.");
            activePlayer = newPlayer;
            CameraAdjust();
            players[activePlayer].hasMoved = false;
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
        ReceiveEvent("The game is over! Everyone's dead, nobody wins! Total number of turns taken: " + turnCount);
    }

    void EndGameWin(Player winner)
    {
        ReceiveEvent("The game is over!" + winner.playerName + " has won! Total number of turns taken: " + turnCount);
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
        players[activePlayer].hasMoved = true;
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


    public string inflictStatus(Player P1, Weapon W1) //P1 is target player and W1 is any weapon, preferably the current weapon of attacking player
    {
        string effectString;
        if (P1.status == State.normal)
        {
            State effect = W1.statusEffect;
            switch (effect)
            {
                case State.normal:
                    effectString = "";
                    return effectString;
                case State.burned:
                    int perc = Random.Range(1, 10);
                    if (perc > 3)
                    {
                        effectString = "";
                        return effectString;
                    }
                    else
                    {
                        P1.status = effect;
                        effectString = P1 + " has been burned!";
                        return effectString;

                    }
                case State.poisoned:
                    perc = Random.Range(1, 10);
                    if (perc > 4)
                    {
                        effectString = "";
                        return effectString;
                    }
                    else
                    {
                        P1.status = effect;
                        effectString = P1 + " has been poisoned!";
                        return effectString;
                    }
                case State.stunned:
                    perc = Random.Range(1, 10);
                    if (perc > 1)
                    {
                        effectString = "";
                        return effectString;
                    }
                    else
                    {
                        P1.status = effect;
                        effectString = P1 + " has been stunned!";
                        return effectString;
                    }
                case State.dead:
                    effectString = "The weapon is broken, too bad.";
                    return effectString;
            }
        }
        effectString = P1 + "already has a status condition.";
        return effectString;
    }

    public void startCombat(Player P1, Player P2)    //this should be called after checking for range from the user to an enemy, hence P1's range is definitely in range
    // P1 is the player who INITIATES the attack.
    {
        if (P2.status == State.dead)
            return;
        int damage1 = P1.currentWeapon.Hit();
        int damage2 = 0;
        string b1 = " " + inflictStatus(P2, P1.currentWeapon) + "\n"; // inflict status, store event string
        string b2 = "";


        if (checkRangeForEnemy(P2, P1))
        {
            damage2 = P2.currentWeapon.Hit();
            b2 = " " + inflictStatus(P1, P2.currentWeapon);
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
        P2.health = P2.health - damage1;
        if (P2.health > 100)
            P2.health = 100;
        P1.health = P1.health - damage2;
        if (P1.health > 100)
            P1.health = 100;

        if (P2.health <= 0)
            P2.status = State.dead;
        if (P1.health <= 0)
            P1.status = State.dead;

        string a1 = P1.playerName + " dealt " + damage1 + " damage to " + P2.playerName + " with the " + P1.currentWeapon.finalName + ".";
        damText1 = a1;
        string a2 = "";
        if (damage2 > 0)
        {
            a2 = P2.playerName + " dealt " + damage2 + " damage to " + P1.playerName + " with the " + P2.currentWeapon.finalName + ".";
            damText2 = a2;
        }
       if (damage2 > damage1)
        {
            loot = P1.rubies * (decimal).45;
            P1.rubies = P1.rubies - loot;
            P2.rubies = P2.rubies + loot;
        }
       if (damage1 > damage2)
        {
            loot = P2.rubies * (decimal).45;
            P2.rubies = P2.rubies - loot;
            P1.rubies = P1.rubies + loot;
        }
        ReceiveEvent(a1 + b1 + a2 + b2);
        combatFlag = true;
           
    }



}
