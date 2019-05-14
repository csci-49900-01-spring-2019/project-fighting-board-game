using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Player> players; // We should instantiate this on runtime instead of hardcoding the list. 
    public List<Camera> cameras;
    public Weapon_List listOfWeapons;
    public Light dayTime; // disable to make night-time. 
    public float timeUntilGameStart = 30f;
    public bool gameOver;
    public int activePlayer;
    public int activeCamera;
    public int turnCount;
    public int wepDrawStart = 0;
    public int wepDrawEnd = 4;
    public string lastEvent;
    public bool eventFlag;
    public bool combatFlag;
    public bool isGameStarted = false;
    public Combat combatSystem;
    public string statText1 = "s1";
    public string statText2 = "s2";
    public string damText1 = "d1";
    public string damText2 = "d2";
    public GameObject fightParticle;
    public GameObject playerPrefab;
    private object photonEvent;

    // Start is called before the first frame update
    void Start()
    {
        // Adds player to master client, then calls event on all other receivers to add player. Only used this so we can cache the function call
        
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("I AM THE MASTER CLIENT!!" + PhotonNetwork.LocalPlayer.ActorNumber);
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == -1)
        {
            Debug.Log("LOCAL BUILD");
            addPlayer("tobob");
            addPlayer("bob");
        } else
        {
            Debug.Log("ONLINE BUILD");
            Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                addPlayer(players[i].NickName);
            }
           
        }
        gameOver = false;
        turnCount = 1;
        activePlayer = 0;
        activeCamera = 7;
        cameras[activeCamera].enabled = true;
        Debug.Log("active player = " + activePlayer);
        players[activePlayer].StartCoroutine("TakeTurn");
    }



    public void addPlayer(string username)
    {
        if (players.Count >= 4)
        {
            Debug.Log("Maximum Players Allowed");
            return;
        }

        
        GameObject newPlayer = Instantiate(playerPrefab);

        Player newPlayerScript = newPlayer.GetComponent<Player>();
        if (players.Count == 0)
        {
            GameObject spawnTile = GameObject.Find("Player1Start");
            newPlayer.transform.position = spawnTile.transform.position;
            newPlayerScript.current_tile = spawnTile.GetComponent<GameTile>();
        } else if (players.Count == 1)
        {
            GameObject spawnTile = GameObject.Find("Player2Start");
            newPlayer.transform.position = spawnTile.transform.position;
            newPlayerScript.current_tile = spawnTile.GetComponent<GameTile>();
        } else if (players.Count == 2)
        {
            GameObject spawnTile = GameObject.Find("Player3Start");
            newPlayer.transform.position = spawnTile.transform.position;
            newPlayerScript.current_tile = spawnTile.GetComponent<GameTile>();
        } else if (players.Count == 3)
        {
            GameObject spawnTile = GameObject.Find("Player4Start");
            newPlayer.transform.position = spawnTile.transform.position;
            newPlayerScript.current_tile = spawnTile.GetComponent<GameTile>();
        }
        //GameObject newPlayer = Instantiate(playerPrefab);
        newPlayerScript.my_die = GameObject.Find("Die").GetComponent<Dice>();
        newPlayerScript.name = username;
        newPlayer.transform.parent = base.transform;
    
        players.Add(newPlayerScript);
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
            sendMovementEvent(activePlayer, backTile.transform.position);
            players[activePlayer].current_tile = backTile;
            TileEffect();
        }
        else // if player has selected forward tile
        {
            sendMovementEvent(activePlayer, forwardTile.transform.position);
            players[activePlayer].current_tile = forwardTile;
            TileEffect();
        }
        forwardTile.DeactivateOutline();
        backTile.DeactivateOutline();
    }

    public void sendMovementEvent(int player, Vector3 target)
    {
        Debug.Log("Raising movement event!");

        byte evCode = (byte)PhotonEventCodes.movement;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        object[] data = new object[] { player, target };

        PhotonNetwork.RaiseEvent(evCode, data, raiseEventOptions, sendOptions);

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
            players[activePlayer].rubies = players[activePlayer].rubies + n;
            ReceiveEvent(players[activePlayer].playerName + " landed on a ruby mine and mined " + n + " rubies!");
        }
    }

    void ChangePlayer()
    {
        CheckGameStatus();
        if (!gameOver)
        { 
            int newPlayer = (activePlayer + 1) % (players.Count);
            while (players[newPlayer].status == State.dead)
            {
                newPlayer = (newPlayer + 1) % (players.Count);
            }
            if (newPlayer < activePlayer) // we are back to beginning of player list, turn has passed
            {
                turnCount += 1;
                if (turnCount % players.Count*3 == 0 && dayTime != null)
                {
                    if (dayTime.enabled)
                    {
                        dayTime.enabled = false;
                    } else
                    {
                        dayTime.enabled = true;
                    }
                    // TURN TO NIGHT-TIME!
                }
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

            // Fix for local too!
            if (PhotonNetwork.LocalPlayer.ActorNumber != -1) {
                CameraAdjustNetworking(newPlayer);
            } else
            {
                CameraAdjust();
            }
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

    void CameraAdjustNetworking(int activeCamera)
    {
        byte evCode = (byte)PhotonEventCodes.updateCamera;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        object[] data = new object[] { activeCamera };

        PhotonNetwork.RaiseEvent(evCode, data, raiseEventOptions, sendOptions);
        Debug.Log("Sent event camera!");
    }

    public void CameraAdjust()
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
                        P1.statusTimer = 3;
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
                        P1.statusTimer = 3;
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
                        P1.statusTimer = 1;
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

        // Spawn particle on fight!
        Instantiate(fightParticle, P1.transform);

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
        if (PhotonNetwork.LocalPlayer.ActorNumber != -1)
        {
            P2.PlayerAttackedNetworking(damage1);
        } else
        {
            P2.PlayerAttacked(damage1);
        }
        
        if (P2.health > 100)
            P2.health = 100;
        if (PhotonNetwork.LocalPlayer.ActorNumber != -1)
        {
            P1.PlayerAttackedNetworking(damage2);
        }
        else
        {
            P1.PlayerAttacked(damage2);
        }
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
        ReceiveEvent(a1 + b1 + a2 + b2);
        combatFlag = true;
           
    }



}
