using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiceUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theDice = GameObject.FindObjectOfType<Dice>();
    }
    Dice theDice;
    // Update is called once per frame
    void Update()
    {
        if (theDice.roll_processed == true)
        {
            GetComponent<Text>().text = "Dice Roll = ?";
        }
        else
        {
            GetComponent<Text>().text = "Dice Roll = " + theDice.roll;
        }
    }
}
