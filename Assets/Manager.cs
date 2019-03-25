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
    public Combat combatSystem;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Entered start.");
        gameOver = false;
        turnCount = 1;
        activePlayer = 0;
        activeCamera = 0;
        players[activePlayer].StartCoroutine("TakeTurn");
    }

    // Update is called once per frame
    void Update()
    {
        //CameraAdjust();
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
        if (players[activePlayer].current_tile.tile_type == "Weapon")
        {
            int n = Random.Range(0, 25);
            Weapon draw = listOfWeapons.wepList[n];

            //players[activePlayer].inventory.Add(draw);
            players[activePlayer].currentWeapon = draw;
            Debug.Log("You have landed on Weapons tile!" + " You drew a " + draw.finalName);

        }
        else if (players[activePlayer].current_tile.tile_type == "Heal")
        {
            int n = Random.Range(10, 41);
            players[activePlayer].health = players[activePlayer].health + n;
            if (players[activePlayer].health > 100)
                players[activePlayer].health = 100;
            Debug.Log("You have landed on a Healing tile!" + " You healed " + n);
        }
    }

    void ChangePlayer()
    {
        int newPlayer = (activePlayer+1) % (players.Count);
        if (newPlayer == 0) newPlayer++;
        Debug.Log(newPlayer);
        activePlayer = newPlayer;
        CameraAdjust();
        players[activePlayer].StartCoroutine("TakeTurn");
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

    void HandleCollision(int index)
    {
        for (int n = 0; n < players.Count; n++)
        {
            if (n != index && players[n].current_tile == players[index].current_tile)
            {
                players[n].AdjustPosition();
                players[index].AdjustPosition();
                startCombat(players[index], players[n]);
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

    public bool inflictStatus(Player P1, Weapon W1) //P1 is target player and W1 is any weapon, preferably the current weapon of attacking player
    {
        if (P1.status == State.normal)
        {
            State effect = W1.statusEffect;

            switch (effect)
            {
                case State.normal:
                    Debug.Log("Your weapon has not status effect");
                    return false;
                case State.burned:
                    int perc = Random.Range(1, 10);
                    if (perc > 3)
                    {
                        Debug.Log("You have failed to burn on the target");
                        return false;
                    }
                    else
                    {
                        P1.status = effect;
                        Debug.Log("You have burned the target");
                        return true;
                    }
                case State.poisoned:
                    perc = Random.Range(1, 10);
                    if (perc > 4)
                    {
                        Debug.Log("You have failed to poison the target");
                        return false;
                    }
                    else
                    {
                        P1.status = effect;
                        Debug.Log("You have poisoned the target");
                        return true;
                    }
                case State.stunned:
                    perc = Random.Range(1, 10);
                    if (perc > 1)
                    {
                        Debug.Log("You have failed to stun the target");
                        return false;
                    }
                    else
                    {
                        P1.status = effect;
                        Debug.Log("You have stunned the target; lucky");
                        return true;
                    }
                case State.dead:
                    Debug.Log("Your target is dead, hasn't he suffered enough?");
                    return false;
            }
        }
        Debug.Log("Target Player already has a status condition");
        return false;
    }

    public void startCombat(Player P1, Player P2)    //this should be called after checking for range from the user to an enemy, hence P1's range is definitely in range
    {

        int damage1 = P1.currentWeapon.Hit();
        int damage2 = 0;

        inflictStatus(P2, P1.currentWeapon);
        if (checkRangeForEnemy(P2, P1))
        {
            damage2 = P2.currentWeapon.Hit();
            inflictStatus(P1, P2.currentWeapon);
        }
        //can modify damage1 and damage2 based on current tiles or otherwise
        P2.health = P2.health - damage1;
        if (P2.health > 100)
            P2.health = 100;
        P1.health = P1.health - damage2;
        if (P1.health > 100)
            P1.health = 100;
    }



}
