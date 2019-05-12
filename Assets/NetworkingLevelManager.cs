using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;
using Photon.Pun;

public enum PhotonEventCodes
{
    takeDamage = 0,
    updateCamera = 1,

}

public class NetworkingLevelManager : Photon.Pun.MonoBehaviourPun
{
    // players!
    public List<GameObject> players;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("Event Received!");
        byte eventCode = photonEvent.Code;

        // Checks codes. 
        if (eventCode == (byte)PhotonEventCodes.takeDamage)
        {
            Debug.Log("Event received! TAKE DAMAGE");

            object[] data = (object[])photonEvent.CustomData;
            // Get corresponding player!
            Player playerScript = players[(int)data[0]-1].GetComponent<Player>();
            // Call the damage script.
            playerScript.PlayerAttacked((int)data[1]);
        }

        if (eventCode == (byte)PhotonEventCodes.updateCamera)
        {
            Debug.Log("Changing camera!");
            object[] data = (object[])photonEvent.CustomData;
            manager.activePlayer = (int)data[0];
            manager.CameraAdjust();
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
