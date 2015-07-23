using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OnlineGameController : Photon.PunBehaviour
{
    private GameObject winScreen;
    private GameObject loseScreen;
    private Sound_Manager sound_manager;
    public GameObject PlayerControls;
    public static bool gameStarted = false;
    public static bool gameEnded = false;
    //bool win = false;
    int unitsCount;
    public GameObject PauseMenu;

    void Awake()
    {
        Time.timeScale = 1.0f;
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>(); // gets the sound sources
        winScreen = GameObject.FindGameObjectWithTag("Win_Screen");
        loseScreen = GameObject.FindGameObjectWithTag("Lose_Screen");
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
        unitsCount = PlayerControls.GetComponent<PlayerController>().allSelectableUnits.Count;
        if (unitsCount <= 0 && gameStarted && !gameEnded)
        {
            photonView.RPC("GameEnd", PhotonTargets.AllViaServer, null);
        }
        if (gameEnded)
        {
            if (unitsCount > 0)
            {
                unitsCount = 1;
            }
            else
            {
                unitsCount = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //PhotonNetwork.LeaveRoom();
            PauseMenu.SetActive(true);
        }
        if (gameEnded && Input.GetKeyUp(KeyCode.Return))
        {
           PhotonNetwork.LeaveRoom();
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        
    }

    [PunRPC]
    public void GameEnd()
    {
        gameEnded = true;
        Time.timeScale = 0.0f;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        // We own this player: send the others our data
    //        stream.SendNext(gameEnded);

    //    }
    //    else
    //    {
    //        // Network player, receive data
    //        this.gameEnded = (bool)stream.ReceiveNext();
    //    }
    //}
    void OnGUI()
    {
        if (gameEnded)
        {

            //if (unitsCount <= 0)
            //{
            //    GUI.BeginGroup(new Rect(Screen.width * 0.5f - 300, Screen.height * 0.5f - 100, 500, 300));
            //    GUI.Box(new Rect(0, 0, 500, 300), "\n\n\n\n\n\n\nYou Lose\nPress Enter to Continue");
            //    GUI.EndGroup();
            //}
            //else
            //{
            //    GUI.BeginGroup(new Rect(Screen.width * 0.5f - 300, Screen.height * 0.5f - 100, 500, 300));
            //    GUI.Box(new Rect(0, 0, 500, 300), "\n\n\n\n\n\n\nYou Win\nPress Enter to Continue");
            //    GUI.EndGroup();
            //}
        }
    }

    void FixedUpdate()
    {

    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        if (gameEnded)
        {
            if (unitsCount <= 0)
            {
                Show_LoseScreen();
            }
            else
            {
                Show_WinningScreen();
            }
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {

        base.OnPhotonPlayerConnected(newPlayer);
        InitPlayer();
        SpawnPlayerUnits();
       
        
        Invoke("InitSync", 2.0f);
    }

    public new void OnLeftRoom()
    {
        //gameStarted = false;
        //gameEnded = true;
        
        Application.LoadLevel("Multiplayer_Lobby");
    }

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        switch (info.photonView.gameObject.tag)
        {
            case "Protein":
                PlayerControls.GetComponent<PlayerController>().AddNewProtein(info.photonView.gameObject.GetComponent<Protein>());
                break;
            case "Unit":
                PlayerControls.GetComponent<PlayerController>().AddNewCell(info.photonView.gameObject.GetComponent<BaseCell>());
                break;
            default:
                break;
        }
    }

    void InitSync()
    {
        GameObject[] tmpArr = GameObject.FindGameObjectsWithTag("Unit"); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            PlayerControls.GetComponent<PlayerController>().AddNewCell(item.GetComponent<BaseCell>());
        }
        tmpArr = GameObject.FindGameObjectsWithTag("Protein"); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            PlayerControls.GetComponent<PlayerController>().AddNewProtein(item.GetComponent<Protein>()); // Add the cell to the players controllable units
        }
        gameStarted = true;
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
        PhotonNetwork.InstantiateSceneObject("Protein", Vector3.right * 10, Quaternion.Euler(90, 0, 0), 0, null).GetComponent<Protein>();
    }

    void SpawnPlayerUnits()
    {
        
        object[] isSingleplayer = new object[1];
        isSingleplayer[0] = (bool)false;
        PhotonNetwork.Instantiate("StemCell", Vector3.right * PhotonNetwork.player.ID, Quaternion.Euler(90, 0, 0), 0, isSingleplayer);
        //if (false/*PhotonNetwork.player.ID == 1*/)
        //{
        //    PlayerControls.GetComponent<PlayerController>().AddNewCell(PhotonNetwork.Instantiate("ColdCell", Vector3.right * PhotonNetwork.player.ID, Quaternion.Euler(90, 0, 0), 0, isSingleplayer).GetComponent<BaseCell>());
        //}
    }

    void Rematch()
    {
        if (true)
        {
            ResetPlayers();
        }
    }


    void Show_WinningScreen()
    {
        winScreen.SetActive(true);

        if (!sound_manager.win_music.isPlaying)
        {
            sound_manager.win_music.Play();
        }
        winScreen.GetComponentInChildren<Image>().enabled = true;
        Image[] test = winScreen.GetComponentsInChildren<Image>();

        foreach (Image img in test)
        {
            img.enabled = true;

        }

        Time.timeScale = 0.0f;
        this.gameObject.SetActive(false);
        Invoke("Disconnect", 2.0f);
    }
    void Show_LoseScreen()
    {

        loseScreen.SetActive(true);
        if (!sound_manager.lose_music.isPlaying)
        {
            sound_manager.lose_music.Play();
        }
        loseScreen.GetComponentInChildren<Image>().enabled = true;

        Image[] test = loseScreen.GetComponentsInChildren<Image>();

        foreach (Image img in test)
        {
            img.enabled = true;

        }

        this.gameObject.SetActive(false);
        Invoke("Disconnect", 2.0f);

    }

}