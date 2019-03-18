using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//    public enum State { normal, burned, poisoned, stunned, dead }; 
// create more adjective fields to modify weapon damage or range, the one named "adjective" will represent the first adjective
public class Weapon : MonoBehaviour
{
    public string name;
    public string adjective;
    public string adjective2;
    public string finalName;
    public int dRangeStart;
    public int dRangeLimit;
    public int range;
    public State statusEffect;
    public int rank;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Weapon(string nam, string adj1, string adj2) //can be changed to allow for adjectives after prototyping
    {
        name = nam;
        adjective = adj1;
        adjective2 = adj2;
        SetVarianceName(nam);
        SetVarianceAdj1(adj1);
        SetVarianceAdj2(adj2);
        finalName = adjective + " " + adjective2 + " " + name;
        rank = 0;
    }

    void SetVarianceName(string name)
    {
        switch (name)
        {
            case "sword":
                dRangeStart = 10;
                dRangeLimit = 16;
                range = 0;
                break;
            case "hammer":
                dRangeStart = 8;
                dRangeLimit = 18;
                range = 0;
                break;
            case "stick":
                dRangeStart = 1;
                dRangeLimit = 6;
                range = 0;
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
                break;
            case "poisoning":
                statusEffect = State.poisoned;
                break;
            case "stunning":
                statusEffect = State.stunned;
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
                break;
            case "short":
                if (range != 0)
                {
                    range--
                }
                break;
            case "strong":
                dRangeStart = dRangeStart + 3;
                dRangeLimit = dRangeLimit + 5;
                break;
            case "weak":
                if (dRangeStart > 4)
                    dRangeStart = dRangeStart - 4;
                dRangeLimit = dRangeLimit - 6;
                break;
            case "quick":
                dRangeStart = dRangeStart + 2;
                break;
            case "grand":
                if (dRangeStart > 5)
                    dRangeStart = dRangeStart - 5;
                dRangeLimit = dRangeLimit + 10;
                break;
            case "critical":
                dRangeLimit = dRangeLimit + dRangeLimit;
                break;
            case "broken":
                dRangeStart = 1;
                dRangeLimit = 2;
                break;
            case "game breaker":
                dRangeLimit = 50;
                dRangeStart = dRangeLimit;
                break;
            case "chance":
                dRangeStart = -25;
                dRangeLimit = 100;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
