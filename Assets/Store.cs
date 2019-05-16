using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindObjectOfType<Manager>();
        //StoreScreen = GameObject.Find("StoreCanvas");
        wepStart = 5;
        wepEnd = 9;
    }
    Manager theManager;
    WeaponPanel childScript;
    //GameObject StoreScreen;
    // Update is called once per frame
    void Update()
    {

    }

    public int wepStart = 5;
    int wepEnd;

    public void SetupStore()
    {
        childScript = GameObject.Find("Weapon1").GetComponent<WeaponPanel>(); ;
        childScript.assignSale();
        childScript.updateText();
        childScript.updateButton();
        childScript = GameObject.Find("Weapon2").GetComponent<WeaponPanel>();
        childScript.assignSale();
        childScript.updateText();
        childScript.updateButton();
        childScript = GameObject.Find("Weapon3").GetComponent<WeaponPanel>();
        childScript.assignSale();
        childScript.updateText();
        childScript.updateButton();
    }

    public void OpenStore()
    {
        if (theManager.wepDrawStart == 20)
        {
            wepStart = 20;
            wepEnd = 24;
        }
        else
        {
            wepStart = theManager.wepDrawStart + 5;
            wepEnd = theManager.wepDrawEnd + 5;
        }
        childScript = GameObject.Find("Weapon1").GetComponent<WeaponPanel>();
        childScript.updateSale();
        childScript.updateText();
        childScript.updateButton();
        childScript = GameObject.Find("Weapon2").GetComponent<WeaponPanel>();
        childScript.updateSale();
        childScript.updateText();
        childScript.updateButton();
        childScript = GameObject.Find("Weapon3").GetComponent<WeaponPanel>();
        childScript.updateSale();
        childScript.updateText();
        childScript.updateButton();
        GetComponent<Canvas>().enabled = true;
    }
    public void CloseStore()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
