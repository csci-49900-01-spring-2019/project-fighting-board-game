using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    //public string adjective;
    public int dRangeStart;
    public int dRangeLimit;
    public int range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Weapon(string nam) //can be changed to allow for adjectives after prototyping
    {
        name = nam;
        SetVariance(name);
    }

    void SetVariance(name)
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



    // Update is called once per frame
    void Update()
    {
        
    }
}
