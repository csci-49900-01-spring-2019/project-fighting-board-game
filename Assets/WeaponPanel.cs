using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StoreCanvas = GameObject.Find("StoreCanvas");
        //parentScript = GameObject.Find("StoreCanvas").GetComponent<Store>();
        parentScript = StoreCanvas.GetComponent<Store>();
        theManager = GameObject.FindObjectOfType<Manager>();
        text = this.transform.Find("Information").GetComponent<Text>();
        butt = this.transform.Find("Button").GetComponent<Button>();
        //check = parentScript.wepStart;
        //n = Random.Range(check, check + 4);
        //For_Sale = theManager.listOfWeapons.wepList[n];
    }
    Button butt;
    Text text; 
    int n;
    Manager theManager;
    Store parentScript;
    public GameObject StoreCanvas;
    public Weapon For_Sale;
    int check;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buying()
    {
        int index;
        int currentPlayer = theManager.activePlayer;
        Weapon temp = theManager.players[currentPlayer].currentWeapon;
        int tempLen = theManager.players[currentPlayer].inventory.Count;
        for (int i = 0; i < tempLen; i++)
        {
            if (temp == theManager.players[currentPlayer].inventory[i])
            {
                theManager.players[currentPlayer].inventory[i] = For_Sale;
                theManager.players[currentPlayer].currentWeapon = For_Sale;
            }
        }

    }

    public void updateButton()
    {
        int currentPlayer = theManager.activePlayer;
        int availableRubies = theManager.players[currentPlayer].rubies;
        if (availableRubies <= For_Sale.rubies)
       {
            butt.enabled = true;
        }
        else
            butt.enabled = false;
    }

    public void updateText()
    {
        text.text = "Name: " + For_Sale.finalName + '\n' + "Damage: " + For_Sale.dRangeStart + '-' + For_Sale.dRangeLimit + '\n' + "Range: " + For_Sale.range + '\n' + "Rubies: " + For_Sale.rubies;
    }

    public void assignSale()
    {
        check = parentScript.wepStart;
        n = Random.Range(check, check + 4);
        For_Sale = theManager.listOfWeapons.wepList[n];
        //Debug.Log(For_Sale.finalName);
    }

    public void updateSale()
    {
        check = parentScript.wepStart;
        n = Random.Range(check, check + 4);
        For_Sale = theManager.listOfWeapons.wepList[n];
        //Debug.Log(For_Sale.finalName);
    }
}
