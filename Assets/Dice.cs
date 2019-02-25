﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generate a random number between two numbers

public class Dice : MonoBehaviour
{
    public int range_start;
    public int range_end;
    private int last_roll;
    private bool roll_processed;

    // Start is called before the first frame update
    void Start()
    {
        range_start = 1;
        range_end = 6;
        roll_processed = true;
        last_roll = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetRoll()
    {
        if (!roll_processed)
        {
            roll_processed = true;
            return last_roll;
        }
        else
        {
            Debug.Log("You didn't roll, silly!");
            return 0;
        }
    }

    private void OnMouseDown()
    {
        if (roll_processed)
        {
            int my_num = Random.Range(range_start, range_end);
            Debug.Log("Dice rolled, you got " + my_num);
            last_roll = my_num;
            roll_processed = false;
        }
        else
        {
            Debug.Log("Hey, you already rolled!");
        }
    }
}