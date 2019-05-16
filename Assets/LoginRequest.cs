/* Create new C# file */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoginRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public InputField usernameInput;
    public InputField passwordInput;

    Player thePlayer;
    string user = "truly";
    string pass = "potato";
    string authToken = "";
    private string authkey;
    public string url = "http://arcane-forest-85239.herokuapp.com/auth/login";
    // Update is called once per frame

    public void login()
    {
        user = usernameInput.text;
        pass = passwordInput.text;
        StartCoroutine(GetToken());
    }

    public void singlePlayer()
    {
        SceneManager.LoadScene(2);
    }

    void processResponse()
    {
        if (authToken == null)
        {
            Debug.Log("NULL");
        }

        // Lol never do this 
        if (authToken.Length > 20)
        {
            Debug.Log("Success");
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator GetToken()
    {
        Debug.Log("Getting token!");
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
                //{ "auth_token":"eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoxMiwiZXhwIjoxNTU4MDYyMzQyfQ.kWzbNRGY71PocNJeGs6xqCDXhJbvD8zijtcP7FIrZOY"}
                authkey = System.Text.Encoding.UTF8.GetString(postRequest.downloadHandler.data);

                int counter = 0;
                authToken = "";
                for (int i = 0; i < authkey.Length; i++)
                {
                    if (counter > 2 && counter < 4 && authkey[i] != '"')
                    {
                        authToken += authkey[i];
                    }

                    if (authkey[i] == '"')
                    {
                        counter++;
                    }
                }
                
                Debug.Log(authToken);
                
            }
        }
        processResponse();
    }  
}