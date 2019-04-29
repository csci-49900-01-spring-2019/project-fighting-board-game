using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = mainTimer;
        theManager = GameObject.FindObjectOfType<Manager>();
    }
    Manager theManager;
    [SerializeField] public Text uiText;
    [SerializeField] public float mainTimer;
    public float timeLeft;
    // Update is called once per frame
    void Update()
    {
        if (timeLeft >= 0.00)
        { 
            int currentPlayer = theManager.activePlayer; 
            timeLeft -= Time.deltaTime;
            uiText.text = "Time left: " + timeLeft.ToString("F");
            if(theManager.players[currentPlayer].isActive == false)
            {
                timeLeft = mainTimer;
            }
        }
        else if(timeLeft <= 0)
        {
         int currentPlayer = theManager.activePlayer;
         theManager.players[currentPlayer].status = State.dead;
            theManager.players[currentPlayer].EndTurn();
         //theManager.activePlayer = (theManager.activePlayer + 1) % (theManager.players.Count);
         timeLeft = mainTimer;
        }
    }
}
