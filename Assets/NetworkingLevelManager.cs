using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public enum PhotonEventCodes
{
    takeDamage = 0,
    updateCamera = 1,
    addPlayer = 2,
    addPlayerMaster = 3,
    movement = 4,
}

public class NetworkingLevelManager : Photon.Pun.MonoBehaviourPun
{
    // players!
    public List<GameObject> players;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        // Init player on joining room!

        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
        for (int i = 0; i < playerList.Length; i++)
        {
            Debug.Log(playerList[i].ActorNumber);
        }
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

            // More like end of turn script! Not update camera.
        } else if (eventCode == (byte)PhotonEventCodes.updateCamera)
        {
            Debug.Log("Changing camera!");
            object[] data = (object[])photonEvent.CustomData;
            manager.activePlayer = (int)data[0];
            manager.players[manager.activePlayer].hasMoved = false;
            manager.CameraAdjust();
        } else if (eventCode == (byte)PhotonEventCodes.addPlayer)
        {
            Debug.Log("Adding player!");
            object[] data = (object[])photonEvent.CustomData;
            manager.addPlayer((string)data[0]);
        } else if (eventCode == (byte)PhotonEventCodes.addPlayerMaster)
        {
            Debug.Log("Calling add player master!");
            object[] data = (object[])photonEvent.CustomData;
            string username = (string)data[0];

            byte evCode = (byte)PhotonEventCodes.addPlayer;
            // RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient, CachingOption = EventCaching.AddToRoomCache };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            object[] dataSend = new object[] { username };
            PhotonNetwork.RaiseEvent(evCode, dataSend, raiseEventOptions, sendOptions);

            manager.addPlayer(username);

            Debug.Log("Raising add player event!");
        } else if (eventCode == (byte)PhotonEventCodes.movement) {
            Debug.Log("Received movement event!");
            // 0 is player, 1 is position
            object[] data = (object[])photonEvent.CustomData;
            manager.players[(int)data[0]].StartCoroutine("Move", (Vector3)data[1]);
        }
        else {
            Debug.Log("Event received, but does not match any event code. Code = " + eventCode);
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
