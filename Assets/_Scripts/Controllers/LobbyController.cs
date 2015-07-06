using UnityEngine;
using System.Collections;

public class LobbyController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.0");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PhotonNetwork.connectionStateDetailed);
    }

    void OnJoinedLobby()
    {
        if (PhotonNetwork.countOfRooms == 0)
        {
            PhotonNetwork.CreateRoom("Kewlala-SGP", new RoomOptions() { maxPlayers = 2 }, null);
        }
        else
            PhotonNetwork.JoinRandomRoom();
    }

    void OnCreatedRoom()
    {

    }

    void OnJoinedRoom()
    {
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
        Application.LoadLevel("Multiplayer_Level");
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom("Kewlala-SGP", new RoomOptions() { maxPlayers = 2 }, null);
    }
}
