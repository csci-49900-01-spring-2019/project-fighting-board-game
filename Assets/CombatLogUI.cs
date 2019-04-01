using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        combatLog = GameObject.FindObjectOfType<Manager>();
    }
    Manager combatLog;
    // Update is called once per frame
    void Update()
    {
        if (combatLog.showLog)
            GetComponent<text>().text = 
    }
}
