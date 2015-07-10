using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineGameController : Photon.PunBehaviour
{
    public GameObject PlayerControls;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.room.playerCount < 2)
        {
            SpawnSceneObjects();
        }
        else
        {
            InitPlayer();
            SpawnPlayerUnits();
            Invoke("InitSync", 2.0f);
        }
        //PlayerControls.AddComponent<PhotonView>();
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
        InitPlayer();
        SpawnPlayerUnits();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Application.LoadLevel("Multiplayer_Lobby");
    }

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        switch (info.photonView.gameObject.tag)
        {
            case "Protein":
                PlayerControls.GetComponent<OnlinePlayerController>().AddNewProtein(info.photonView.gameObject.GetComponent<Protein>());
                break;
            case "Unit":
                PlayerControls.GetComponent<OnlinePlayerController>().AddNewCell(info.photonView.gameObject.GetComponent<BaseCell>());
                break;
            default:
                break;
        }
    }

    void InitSync()
    {
        GameObject[] tmpArr = GameObject.FindGameObjectsWithTag("Protein"); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            PlayerControls.GetComponent<OnlinePlayerController>().AddNewProtein(item.GetComponent<Protein>()); // Add the cell to the players controllable units
        }
    }

    void InitPlayer()
    {
        PlayerControls.SetActive(true);
        //PlayerControls.GetComponent<PhotonView>().ObservedComponents.Add(PlayerControls.GetComponent<OnlinePlayerController>());
    }

    void ResetPlayers()
    {

    }

    void SpawnSceneObjects()
    {
        PhotonNetwork.InstantiateSceneObject("Protein", Vector3.right * 4, Quaternion.Euler(90, 0, 0), 0, null).GetComponent<Protein>();
    }

    void SpawnPlayerUnits()
    {
        PlayerControls.GetComponent<OnlinePlayerController>().AddNewCell(PhotonNetwork.Instantiate("StemCell", Vector3.right * PhotonNetwork.player.ID, Quaternion.Euler(90, 0, 0), 0).GetComponent<BaseCell>());
    }

    void Rematch()
    {
        if (true)
        {
            ResetPlayers();
        }
    }

}