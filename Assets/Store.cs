using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindObjectOfType<Manager>();
    }
    Manager theManager;
    // Update is called once per frame
    void Update()
    {
        
    }

    int wepStart = 5;
    int wepEnd = 9;

    void OpenStore()
    {
        wepStart = theManager.wepDrawStart + 5;
        wepEnd = theManager.wepDrawEnd + 5;

    }
}
