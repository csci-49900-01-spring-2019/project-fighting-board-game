using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLogUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        combatLog = GameObject.FindObjectOfType<Manager>();
    }
    string player1_CLog;
    string player2_CLog;
    Manager combatLog;
    void update_string()
    {
        player1_CLog = combatLog.statText1 + "\n" + combatLog.damText1 + "\n";
        player2_CLog = combatLog.statText2 + "\n" + combatLog.damText2 + "\n";
    }
    // Update is called once per frame
    void Update()
    {
        if (combatLog.combatFlag == true)
        {
            update_string();
            GetComponent<Text>().text = GetComponent<Text>().text + player1_CLog + player2_CLog;
            combatLog.combatFlag = false;
        }
    }
}