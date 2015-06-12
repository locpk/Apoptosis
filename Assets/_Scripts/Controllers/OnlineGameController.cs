using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineGameController : Photon.MonoBehaviour
{


	void Awake() {
        
    }

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.0");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
       
    }

	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
        
    }

    void OnJoinedLobby()
    {
        if (PhotonNetwork.countOfRooms == 0)
        {
            PhotonNetwork.CreateRoom("Kewlala-SGP", new RoomOptions() { maxPlayers = 2 }, null);
        }
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom("Kewlala-SGP", new RoomOptions() { maxPlayers = 2 }, null);
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

    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
       
    }

    void OnLeftRoom()
    {

        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }


    void OnCreatedRoom()
    {
       
    }

    void InitPlayers()
    {

    }

    void ResetPlayers()
    {

    }

    void SpawnSceneObjects()
    {

    }

    void SpawnPlayerUnits()
    {

    }

    void Rematch()
    {

    }

}