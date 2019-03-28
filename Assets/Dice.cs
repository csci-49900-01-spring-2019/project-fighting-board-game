using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generate a random number between two numbers

public class Dice : MonoBehaviour
{
    public int range_start;
    public int range_end;
    public bool roll_processed;
    public int roll;

    // Start is called before the first frame update
    void Start()
    {
        range_start = 1;
        range_end = 6;
        roll_processed = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
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
    */

    private void OnMouseDown()
    {
        if (roll_processed)
        {
            roll = Random.Range(range_start, range_end);
            Debug.Log("Dice rolled, you got " + roll);
            gameObject.SendMessageUpwards("PlayerSelectSpace", roll);
            roll_processed = false;

        }
        else
        {
            Debug.Log("Hey, you already rolled!");
        }
    }

    public void MakeDieAvailable()
    {
        roll_processed = true;
    }
}
