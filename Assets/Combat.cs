using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Player P1;
    public Player P2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            switch (effect){
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

        int damage1 = P1.currentWeapon.Hit() ;
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
