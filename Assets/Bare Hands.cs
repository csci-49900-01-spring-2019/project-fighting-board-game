using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BareHands
{
    // Start is called before the first frame update
    public string Wname;
    public string adjective;
    public string adjective2;
    public string finalName;
    public int range;
    public int dRangeStart;
    public int dRangeLimit;
    public State statusEffect;
    // public int uses;
    // Start is called before the first frame update
    void Start()
    {
        Wname = "Bare Hands";
        adjective = "";
        adjective2 = "";
        finalName = Wname;
        dRangeStart = 1;
        dRangeLimit = 3;
        range = 0;
        statusEffect = State.normal;

}

    public int Hit()
    {
        int damage = Random.Range(dRangeStart, dRangeLimit);
        Debug.Log("You dealt " + damage + " with the " + finalName);
        return damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
