using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonConnect : MonoBehaviour
{
	public string versionName = "0.1";
    public GameObject sectionView1, sectionView2, sectionView3;
    public Text roomList;
    private void Awake(){
        PhotonNetwork.ConnectUsingSettings(versionName);
        Debug.Log("Connecting to photon...");
	}

    private void OnReceivedRoomListUpdate()
    {
        roomList.text = "";
        foreach (RoomInfo rm in PhotonNetwork.GetRoomList())
        {
            if(rm.PlayerCount!=rm.MaxPlayers)
            roomList.text += "Room: " + rm.name + "- Players: " + rm.PlayerCount + "/" + rm.MaxPlayers+"\n";
        }
    }

    private void OnConnectedToMaster()
    {
        if(!PhotonNetwork.insideLobby)
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("We are connected to master");
        //viewRoomList();
    }
    private void OnJoinedLobby()
    {        
        sectionView1.SetActive(false);
        sectionView2.SetActive(true);
        
        Debug.Log("On Joined Lobby");
    }
    private void OnDisconnectedFromPhoton()
    {
        if (sectionView1.active)
        {
            sectionView1.SetActive(false);
        }
        if (sectionView2.active)
        {
            sectionView2.SetActive(false);
        }
        sectionView3.SetActive(true);
        Debug.Log("Dis from photon services");
    }
}
