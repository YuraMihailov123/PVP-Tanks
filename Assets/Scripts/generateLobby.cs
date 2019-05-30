using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generateLobby : Photon.MonoBehaviour
{
    public Text listPlayers;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            listPlayers.text += "\n "+ PhotonNetwork.playerList[i].NickName + " joined the room";
        }
    }
}
