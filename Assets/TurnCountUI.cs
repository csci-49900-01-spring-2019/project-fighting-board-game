using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TurnCount = GameObject.FindObjectOfType<Manager>();
    }
    Manager TurnCount;
    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Current Turn: " + TurnCount.turnCount;
    }
}
