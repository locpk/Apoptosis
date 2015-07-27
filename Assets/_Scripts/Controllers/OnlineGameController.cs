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
        gameEnded = false;
        gameStarted = false;
        PhotonNetwork.automaticallySyncScene = true;
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
        Application.LoadLevel("Multiplayer_Lobby");

    }

    [PunRPC]
    public void GameStarted()
    {
        gameStarted = true;

    }

    [PunRPC]
    public void GameEnd()
    {
        gameEnded = true;

    }


    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        if (gameEnded)
        {
            gameEnded = false;
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

    void OnGUI()
    {
        if (!gameStarted)
        {
            GUI.BeginGroup(new Rect(Screen.width * 0.5f - 300, Screen.height * 0.5f - 100, 500, 300));
            GUI.Box(new Rect(0, 0, 500, 300), "\n\n\n\n\n\n\nWait for the second player.\n");
            GUI.EndGroup();
        }

    }

    void OnLevelWasLoaded()
    {
        if (PhotonNetwork.player.ID == 2)
        {
            photonView.RPC("GameStarted", PhotonTargets.AllViaServer, null);
        }

    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {

        base.OnPhotonPlayerConnected(newPlayer);
        InitPlayer();
        SpawnPlayerUnits();
        PhotonNetwork.room.open = false;
        PhotonNetwork.room.visible = false;
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

        switch (PhotonNetwork.player.ID)
        {
            case 1:
                PhotonNetwork.Instantiate("StemCell", GameObject.Find("Player - 1").transform.position, Quaternion.Euler(90, 0, 0), 0, new object[] { (bool)false });
                break;
            case 2:
                PhotonNetwork.Instantiate("StemCell", GameObject.Find("Player - 2").transform.position, Quaternion.Euler(90, 0, 0), 0, new object[] { (bool)false });
                break;
            default:
                break;
        }

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