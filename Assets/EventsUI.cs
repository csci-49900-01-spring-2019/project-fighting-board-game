using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsUI : MonoBehaviour
{
    Manager theManager;

    // Start is called before the first frame update
    void Start()
    {
        theManager = FindObjectOfType<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theManager.eventFlag)
        {
            string eventString = theManager.lastEvent;
            theManager.eventFlag = false;
            GetComponent<Text>().text = eventString;
        }
    }
}
