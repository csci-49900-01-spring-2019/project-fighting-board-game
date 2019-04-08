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
        text = GetComponent<Text>();
    }
    Manager combatLog;
    Text text;
    // Update is called once per frame
    void Update()
    {
        string player1_CLog = combatLog.statText1 + "\n" + combatLog.damText1 + "\n";
        string player2_CLog = combatLog.statText2 + "\n" + combatLog.damText2 + "\n";
        text.text = text.text + player1_CLog + player2_CLog;
        
    }
}