using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Player P1;
    public Player P2;
    public bool show_log = false;
    public bool called = false;
    public string statText;
    public string damText1;
    public string damText2;
    public int damageTo1;
    public int damageTo2;

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

    public bool inflictStatus(Player P1, Player P2) //P1 is target player and P2 is attacking player
    {
        if (P1.status == State.normal)
        {
            State effect = P2.currentWeapon.statusEffect;

            switch (effect){
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
    {
        if (P2.status == State.dead)
            return;
        int damage1 = P1.currentWeapon.Hit() ;
        int damage2 = 0;

        inflictStatus(P2, P1);
        if (checkRangeForEnemy(P2, P1))
        {
            damage2 = P2.currentWeapon.Hit();
            inflictStatus(P1, P2);
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
