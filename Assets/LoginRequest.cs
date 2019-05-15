using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class LoginRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindObjectOfType <Player>();
        StartCoroutine(GetToken());
    }
    Player thePlayer;
    string user;
    string pass;
    public string url = "http://arcane-forest-85239.herokuapp.com/auth/login";
    // Update is called once per frame
    IEnumerator GetToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", pass);
        UnityWebRequest postRequest = UnityWebRequest.Post(url, form);
        yield return postRequest.SendWebRequest();
        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.error);
        }
        else
        {
            if (postRequest.isDone)
            {
                thePlayer.auth_key = int.Parse(System.Text.Encoding.UTF8.GetString(postRequest.downloadHandler.data));
            }
        }
    }
}