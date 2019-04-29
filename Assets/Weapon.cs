using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//    public enum State { normal, burned, poisoned, stunned, dead }; 
// create more adjective fields to modify weapon damage or range, the one named "adjective" will represent the first adjective
public class Weapon
{
    public string Wname;
    public string adjective;
    public string adjective2;
    public string finalName;
    public int dRangeStart;
    public int dRangeLimit;
    public int range;
    public State statusEffect;
    public int rank;
    public int rubies;

    // Start is called before the first frame update
    void Start()
    {

    }

    public Weapon(string nam, string adj1, string adj2) //can be changed to allow for adjectives after prototyping
    {
        Wname = nam;
        adjective = adj1;
        adjective2 = adj2;
        SetVarianceName(nam);
        SetVarianceAdj2(adj2);
        SetVarianceAdj1(adj1);
        finalName = adjective + " " + adjective2 + " " + Wname;
        rank = 0;
    }

    void SetVarianceName(string nam)
    {
        switch (nam)
        {
            case "sword":
                dRangeStart = 10;
                dRangeLimit = 16;
                range = 0;
                rubies = 18;
                break;
            case "hammer":
                dRangeStart = 8;
                dRangeLimit = 18;
                range = 0;
                rubies = 13;
                break;
            case "stick":
                dRangeStart = 1;
                dRangeLimit = 6;
                range = 0;
                rubies = 5;
                break;
        }
    }

    void SetVarianceAdj2(string adj)
    {
        switch (adj)
        {
            case "":
                break;
            case "long":
                range++;
                rubies = (int)((rubies * 1.75) + .5);
                break;
            case "short":
                if (range != 0)
                {
                    range--;
                    rubies = (int)((rubies - (rubies * .25)) + .5);
                }
                break;
            case "strong":
                dRangeStart = dRangeStart + 3;
                dRangeLimit = dRangeLimit + 5;
                rubies = (int)((rubies * 1.5) + .5);
                break;
            case "weak":
                if (dRangeStart > 4)
                    dRangeStart = dRangeStart - 4;
                dRangeLimit = dRangeLimit - 6;
                rubies = (int)((rubies - (rubies * .5)) + .5);
                break;
            case "quick":
                dRangeStart = dRangeStart + 2;
                rubies = (int)((rubies * 1.2) + .5);
                break;
            case "grand":
                if (dRangeStart > 5)
                    dRangeStart = dRangeStart - 5;
                dRangeLimit = dRangeLimit + 10;
                rubies = (int)((rubies * 1.4) + .5);
                break;
            case "critical":
                dRangeLimit = dRangeLimit + dRangeLimit;
                rubies = (int)((rubies * 2) + .5);
                break;
            case "broken":
                dRangeStart = 6;
                dRangeLimit = 8;
                rubies = (int)((rubies - (rubies * .9)) + .5);
                break;
            case "game breaker":
                dRangeLimit = 50;
                dRangeStart = dRangeLimit;
                rubies = (int)((rubies * 2.5) + .5);
                break;
            case "chance":
                dRangeStart = -25;
                dRangeLimit = 100;
                rubies = (int)((rubies * 3) +.5);
                break;
        }
    }

    void SetVarianceAdj1(string adj)
    {
        switch (adj)
        {
            case "":
                statusEffect = State.normal;
                break;
            case "burning":
                statusEffect = State.burned;
                rubies = (int)((rubies + (rubies * .5)) + .5);
                break;
            case "poisoning":
                statusEffect = State.poisoned;
                rubies = (int)((rubies + (rubies * .7)) + .5);
                break;
            case "stunning":
                statusEffect = State.stunned;
                rubies = (int)((rubies + (rubies * .9)) + .5);
                break;
        }
    }


    public int Hit()
    {
        int damage = Random.Range(dRangeStart, dRangeLimit);
        //Debug.Log("You dealt " + damage + " with the " + finalName);
        return damage;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
