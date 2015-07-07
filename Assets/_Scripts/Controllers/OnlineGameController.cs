using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineGameController : Photon.MonoBehaviour
{


	void Awake() {
        SpawnSceneObjects();
        InitPlayers();
        SpawnPlayerUnits();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
       
    }

	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
        
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
       
    }

    void OnLeftRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
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