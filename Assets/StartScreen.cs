using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Player aPlayer;
    public GameObject inputScreen;

    // Start is called before the first frame update
    void Start()
    {
        inputScreen.SetActive(true);
        var input = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(delegate {
            aPlayer.playerName = input.text;
            Debug.Log(aPlayer.playerName);
            inputScreen.SetActive(false); // false to hide, true to show
        });
        input.onEndEdit = se;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
