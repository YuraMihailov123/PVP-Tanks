using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class photonHandler : Photon.MonoBehaviour
{
    public Text listPlayers;
    public photonButtons photonB;
    public GameObject mainPlayer;
    public GameObject sp1, sp2;
    //public GameObject textStart, textStartObj;
    private GameObject pl1, pl2;
    public bool isPlayScene = false;
    private void Awake()
    {
        //if(GameObject.Find("photonDontDestroy"))
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        DontDestroyOnLoad(this.transform);
        
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 20;
        
    }
    private void Start()
    {     
        photonB = GameObject.Find("photonScript").GetComponent<photonButtons>();
    }
    

    public void createNewRoom()
    {
        if (photonB.nameInput.text != "")
            PhotonNetwork.player.NickName = photonB.nameInput.text;
        else PhotonNetwork.player.NickName = "Unnamed";
        PhotonNetwork.CreateRoom(photonB.createRoomInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
        
    }

    public void joinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        if (photonB.nameInput.text != "")
            PhotonNetwork.player.NickName = photonB.nameInput.text;
        else PhotonNetwork.player.NickName = "Unnamed";
        PhotonNetwork.JoinOrCreateRoom(photonB.joinRoomInput.text, roomOptions, TypedLobby.Default);
    }

    public void moveScene()
    {        
        PhotonNetwork.LoadLevel("mainGame");
    }
    


    private void OnJoinedRoom()
    {
        Debug.Log("We are connected to the room!" );
        //if (PhotonNetwork.playerList.Length == 2)
        moveScene();
        
       
    }

    private void OnSceneFinishedLoading(Scene scene,LoadSceneMode mode)
    {
        
        Debug.Log(scene.name);
        if (scene.name == "1level")
        {
           
            spawnPlayerLobby();
            
        }
        if (scene.name == "mainGame")
        {
            isPlayScene = true;
            
            spawnPlayer();
            //if(PhotonNetwork.room.PlayerCount==2)
            //textStartObj = PhotonNetwork.Instantiate(textStart.name, textStart.transform.position, textStart.transform.rotation, 0);
            
            
        }
    }
    //public void DeleteTimer()
    //{
    //    PhotonNetwork.Destroy(textStartObj);
       
    //}


    private void OnDisconnectedFromPhoton()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        //PhotonNetwork.playerList.
        PhotonNetwork.LoadLevel("sampleScene");
        Destroy(GameObject.Find("photonDontDestroy"));        

    }

    private void spawnPlayer()
    {
        sp1 = GameObject.Find("sp1");
        sp2 = GameObject.Find("sp2");
        
        mainPlayer.name = "mainPlayer";
        //Debug.Log("createdR" + PhotonNetwork.player.ID % 2);
        if (PhotonNetwork.player.ID % 2 == 0)
        {

            PhotonNetwork.Instantiate(mainPlayer.name, sp1.transform.position, mainPlayer.transform.rotation, 0);
            
        }
        else
        {            
            PhotonNetwork.Instantiate(mainPlayer.name, sp2.transform.position, mainPlayer.transform.rotation, 0);
        }
        
    }
    private void spawnPlayerLobby()
    {
        
        //Debug.Log("createdL"+ PhotonNetwork.player.ID % 2);
        mainPlayer.name = "mainPlayer";

        if (PhotonNetwork.player.ID % 2 == 0)
        {

            pl1 = PhotonNetwork.Instantiate(mainPlayer.name, mainPlayer.transform.position, mainPlayer.transform.rotation, 0);

        }
        else
        {
            pl2 = PhotonNetwork.Instantiate(mainPlayer.name, mainPlayer.transform.position, mainPlayer.transform.rotation, 0);
        }
        

    }
}
