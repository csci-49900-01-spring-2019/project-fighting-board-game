using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using Photon.Pun;
using System.Net;

public class postStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(Upload());
        if (PhotonNetwork.LocalPlayer.ActorNumber != -1)
        thePlayer = manager.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        playersList = manager.players;
        authToken = PlayerPrefs.GetString("auth_token");
        Debug.Log(authToken);
    }
    public Player thePlayer;
    public Manager manager;

    private List<Player> playersList;

    private const string URLgame = "https://arcane-forest-85239.herokuapp.com/api/v1/games";
    private const string URLsession = "https://arcane-forest-85239.herokuapp.com/api/v1/game_sessions";
    private string authToken = "eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoxMiwiZXhwIjoxNTU4MDcxMjE1fQ.Jkr1IUdzD-uTF0y4vSMC9KQJHh1jr7sNxLsHtLGHT4U";

    public void uploadAllState()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            StartCoroutine(UploadAll());
        }
    }

    IEnumerator UploadAll()
    {
        // POST game!
        int gameID = 0;
        int userID = PlayerPrefs.GetInt("user_id");
        string authenticate = "Bearer " + authToken;
        Debug.Log(authenticate);
        WWWForm form = new WWWForm();

        form.AddField("num_turns", 10);
        form.AddField("winner_of_game", "bobobo");
        form.AddField("time_elapsed", 100);

        

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", authenticate);
        WWW www = new WWW(URLgame, form.data, headers);
        yield return www;

        Debug.Log(www.text);

        gameID = parseGameID(www.text);

        // POST Game Sessions! 

        for (int i = 1; i < playersList.Count+1; i++)
        {
            WWWForm form2 = new WWWForm();
            Player currentPlayer = playersList[i - 1];

            form2.AddField("game_id", gameID);
            form2.AddField("user_id", userID);
            form2.AddField("total_damage_taken", currentPlayer.totalDamageTaken);
            form2.AddField("total_damage_dealt", currentPlayer.totalDamageDealt);
            form2.AddField("total_healing", currentPlayer.totalHealing);
            form2.AddField("num_kills", currentPlayer.totalKills);
            form2.AddField("weapons_collected", UnityEngine.Random.Range(1,20));

            Dictionary<string, string> headers2 = new Dictionary<string, string>();
            headers2.Add("Authorization", authenticate);
            WWW www2 = new WWW(URLsession, form2.data, headers2);
            yield return www2;

            Debug.Log(www2.text);
        }

    }


    // the worst code ive ever written
    int parseGameID(string json)
    {
        string id = "";
        int result = 0;
        bool passedTheColon = false;
        for (int i = 0; i < json.Length; i++)
        {
            if (json[i] == ',')
            {
                Int32.TryParse(id,out result);
                return result;
            }

            if (passedTheColon)
            {
                id += json[i];
            }

            if (json[i] == ':')
            {
                passedTheColon = true;
            }
            
        }
        return 100;
    }
}