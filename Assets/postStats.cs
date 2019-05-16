using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using Photon.Pun;

public class postStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(Upload());
        if (PhotonNetwork.LocalPlayer.ActorNumber != -1)
        thePlayer = manager.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
    }
    public Player thePlayer;
    public Manager manager;

    private const string URL = "https://arcane-forest-85239.herokuapp.com/api/v1/game_sessions";
    IEnumerator Upload()
    {
        if (thePlayer.status == State.dead)
        {
            WWWForm form = new WWWForm();
            form.AddField("app_key", thePlayer.auth_key);
            form.AddField("Rank", thePlayer.finalRank);
            form.AddField("totalDamageDealt", thePlayer.totalDamageDealt);
            form.AddField("totalDamageTaken", thePlayer.totalDamageTaken);
            form.AddField("totalHealing", thePlayer.totalHealing);
            form.AddField("totalKills", thePlayer.totalKills);
            UnityWebRequest postRequest = UnityWebRequest.Post(URL, form);
            yield return postRequest.SendWebRequest();
            if (postRequest.isNetworkError || postRequest.isHttpError)
            {
                Debug.Log(postRequest.error);
            }
            else
            {
                Debug.Log("Upload complete");
            }
        }
    }
}