using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Example : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }else{

            //textc.text.text = "Connecting...";
        		
            // #Critical, we must first and foremost connect to Photon Online Server.
            //PhotonNetwork.GameVersion = this.gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
