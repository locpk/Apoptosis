using UnityEngine;
using System.Collections;

public class LobbyController : Photon.PunBehaviour
{

    public UnityEngine.UI.Text ConnectionStatusText;
    public GameObject RandomJoinButton;
    System.Collections.Generic.List<GameObject> roomButtons = new System.Collections.Generic.List<GameObject>();
    public GameObject CreateRoomPanel;
    public UnityEngine.UI.InputField CreateRoomInput;

    // Use this for initialization
    void Start()
    {
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("0.0");
        }
        CreateRoomPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ConnectionStatusText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public void Disconnect()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel("MainMenu");
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }


    }

    public void JoinRandomRoom()
    {
        base.OnJoinedLobby();
        if (PhotonNetwork.countOfRooms == 0)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 2 }, null);
        }

        else
            PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        bool unique = true;
        foreach (RoomInfo item in PhotonNetwork.GetRoomList())
        {
            if (item.name == CreateRoomInput.text)
            {
                unique = false;
            }
        }
        if (unique)
        {
            PhotonNetwork.CreateRoom(CreateRoomInput.text, new RoomOptions() { maxPlayers = 2 }, null);
        }
        else
            CreateRoomInput.text = "That room name already exists";
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        RandomJoinButton.SetActive(true);
        InvokeRepeating("RefreshOpenRooms", 0.0f, 5.0f);
    }

    public void RefreshOpenRooms()
    {
        for (int i = 0; i < roomButtons.Count; )
        {
            Destroy(roomButtons[i]);
            roomButtons.RemoveAt(i);
        }
        GameObject joinButton = GameObject.Find("RoomJoinButton");
        GameObject thePanel = GameObject.Find("RoomListPanel");
        Vector3 scale;
        scale.x = 3.616198f;
        scale.y = 3.616198f;
        scale.z = 1.0f;
        int count = 0;
        foreach (RoomInfo item in PhotonNetwork.GetRoomList())
        {
            joinButton = Instantiate(joinButton, Vector3.zero, Quaternion.identity) as GameObject;
            joinButton.transform.SetParent(thePanel.transform, false);
            joinButton.GetComponentInChildren<UnityEngine.UI.Text>().transform.localScale = scale;
            joinButton.GetComponentInChildren<UnityEngine.UI.Text>().text = item.name;
            joinButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(item.name));
            roomButtons.Add(joinButton);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (IsInvoking("RefreshOpenRooms"))
        {
            CancelInvoke("RefreshOpenRooms");
        }
        if (Application.isEditor)
        {

        }
        else
        {
            if (PhotonNetwork.room.playerCount >= 2)
            {
                PhotonNetwork.room.open = false;
            }
        }
        PhotonNetwork.isMessageQueueRunning = false;
        GetComponent<Menu>().LoadScene("Multiplayer_Level");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 2 }, null);
    }
}
