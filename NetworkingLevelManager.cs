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
    recieveEvent = 5,
    tileList = 6,
    tileEvent = 7,
    wepList = 8,
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
            Player playerScript = manager.players[(int)data[0]].GetComponent<Player>();
            Debug.Log("Dealing damage to " + (int)data[0]);
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
            manager.players[manager.activePlayer].StartCoroutine("TakeTurn");
        } else if (eventCode == (byte)PhotonEventCodes.addPlayer)
        {
            /*
            Debug.Log("Adding player!");
            object[] data = (object[])photonEvent.CustomData;
            manager.addPlayer((string)data[0]);
            */
        } else if (eventCode == (byte)PhotonEventCodes.addPlayerMaster)
        {
            /*
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
            */
            Debug.Log("Raising add player event!");
        } else if (eventCode == (byte)PhotonEventCodes.movement) {
            Debug.Log("Received movement event!");
            // 0 is player, 1 is position
            object[] data = (object[])photonEvent.CustomData;
            manager.players[(int)data[0]].StartCoroutine("Move", (Vector3)data[1]);
            string tileName = (string)data[2];
            GameTile nextTile = GameObject.Find(tileName).GetComponent<GameTile>();
            manager.players[(int)data[0]].current_tile = nextTile;
        } else if (eventCode == (byte)PhotonEventCodes.recieveEvent)
        {
            Debug.Log("Received Event Change");
            object[] data = (object[])photonEvent.CustomData;

            manager.ReceiveEvent((string)data[0],false);
            if ((bool)data[1])
            {
                manager.combatFlag = true;
            }

        } else if (eventCode == (byte)PhotonEventCodes.tileList)
        {
            Debug.Log("Received tile list event");
            object[] data = (object[])photonEvent.CustomData;

            for (int i = 0; i < 28; i++)
            {
                manager.tempTile.tile_type = (TileType)data[i];
                manager.tempTile.InitializeTile();
                manager.tempTile = manager.tempTile.next_tile;
            }
        } else if (eventCode == (byte)PhotonEventCodes.tileEvent)
        {
            Debug.Log("Received tile event");
            object[] data = (object[])photonEvent.CustomData;
            manager.StartCoroutine("networkTileEffect", (int)data[0]);
        } else if (eventCode == (byte)PhotonEventCodes.wepList)
        {
            Debug.Log("Received weapon list event");
            object[] data = (object[])photonEvent.CustomData;
            manager.listOfWeapons.wepList = new List<Weapon> { };
            for (int i = 0; i < 75; i++)
            {
                manager.listOfWeapons.wepList.Add(new Weapon((string)data[i], (string)data[i + 1], (string)data[i + 2]));
                i = i + 2;
            }
            //manager.listOfWeapons.wepList = manager.listOfWeapons.wepList.OrderBy(x => x.dRangeLimit).ToList();
            manager.listOfWeapons.rankList();
            /*for (int i = 0; i < manager.listOfWeapons.wepList.Count; i++)
            {
                Debug.Log(manager.listOfWeapons.wepList[i].finalName + " " + i);
            }*/
        } else {
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
