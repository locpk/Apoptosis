using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineGameController : Photon.PunBehaviour
{
    public GameObject PlayerControls;

    void Awake()
    {
        if (PhotonNetwork.room.playerCount < 2)
        {
            SpawnSceneObjects();
        }
        else
        {
            InitPlayers();
            SpawnPlayerUnits();
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    void FixedUpdate()
    {

    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {

    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        InitPlayers();
        SpawnPlayerUnits();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Application.LoadLevel("Multiplayer_Lobby");
    }

    void InitPlayers()
    {
        PlayerControls.SetActive(true);
    }

    void ResetPlayers()
    {

    }

    void SpawnSceneObjects()
    {
        PhotonNetwork.InstantiateSceneObject("Protein", Vector3.right * 4, Quaternion.Euler(90, 0, 0), 0, null);
    }

    void SpawnPlayerUnits()
    {
        PlayerControls.GetComponent<PlayerController>().AddNewCell(PhotonNetwork.Instantiate("StemCell", Vector3.right * PhotonNetwork.player.ID, Quaternion.Euler(90, 0, 0), 0).GetComponent<BaseCell>());
    }

    void Rematch()
    {
        if (true)
        {
            ResetPlayers();
        }
    }

}