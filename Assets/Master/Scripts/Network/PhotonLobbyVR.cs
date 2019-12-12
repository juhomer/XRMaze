using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UIElements.Button;

public class PhotonLobbyVR : MonoBehaviourPunCallbacks
{
    public static PhotonLobbyVR lobbyVrInstance;
    public GameObject playButton;
    public GameObject waitButton;
    public bool VrPlayerSeesJoinButton;
    
    private void Awake ()
    {
#if UNITY_IOS && !UNITY_EDITOR || UNITY_ANDROID && !UNITY_EDITOR
        SceneManager.LoadScene("NetworkTesting");
#endif
        lobbyVrInstance = this;
    }

    private void Start ()
    {
        playButton.SetActive (false);
        waitButton.SetActive (true);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "ru";
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings ();    // Connects to photon master servers
        }
    }

    public override void OnConnectedToMaster ()
    {
        Debug.Log(" VR Player has connected to the Photon master server");
        PhotonNetwork.JoinLobby();      //We have to join a lobby before being able to list rooms, at least thats what somebody (once) told me
        PhotonNetwork.AutomaticallySyncScene = false;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX

        Debug.Log ($"Current platform is {Application.platform}");
        if (VrPlayerSeesJoinButton)
        {
            waitButton.SetActive (false);
            playButton.SetActive (true);
        }
#endif
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join random game, but failed. There must be no open games available.");
        Debug.Log ($"Error message: {message}");
        //CreateRoom();
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name.");
        //CreateRoom();
    }
    
    public void OnVrPlayerReadyToJoinGame ()
    {
        VrPlayerSeesJoinButton = true;
        waitButton.SetActive (false);
        playButton.SetActive (true);
    }
}
