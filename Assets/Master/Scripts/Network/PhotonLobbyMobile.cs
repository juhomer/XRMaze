﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PhotonLobbyMobile : MonoBehaviourPunCallbacks
{
    public static PhotonLobbyMobile LobbyMobileInstance;

    [SerializeField] private ButtonBehaviour  mainCanvasButtonBehaviour;
    [FormerlySerializedAs ("WaitText")] public GameObject waitText;
    [FormerlySerializedAs ("RoomsCanvas")] public GameObject roomsCanvas;
    private Vector3 canvasLocation;
    RoomListingMenu lister;
    

    public Button CreateRoomButton;

    [FormerlySerializedAs ("roomName")] [FormerlySerializedAs ("_roomName")] [SerializeField]
    private InputField roomNameInputField;

    public bool VrPlayerSeesJoinButton; //default false, true when at least one room exists.

    private void Awake()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        SceneManager.LoadScene ("VRMenu");
#endif
        LobbyMobileInstance = this;
        lister = FindObjectOfType<RoomListingMenu>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCanvasButtonBehaviour.ChangeButtonState (ButtonBehaviour.State.Offline);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "ru";
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings ();    // Connects to photon master servers
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Mobile Player has connected to the Photon master server");
        PhotonNetwork.JoinLobby();      //We have to join a lobby before being able to list rooms, at least thats what somebody (once) told me
        PhotonNetwork.AutomaticallySyncScene = false;
#if UNITY_IOS && !UNITY_EDITOR || UNITY_ANDROID && !UNITY_EDITOR
        mainCanvasButtonBehaviour.ChangeButtonState (ButtonBehaviour.State.Play);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX

        Debug.Log ($"Current platform is {Application.platform}");
        if (VrPlayerSeesJoinButton)
        {
            mainCanvasButtonBehaviour.ChangeButtonState (ButtonBehaviour.State.Play);
        }
#endif
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Playbutton was clicked");
        mainCanvasButtonBehaviour.ChangeButtonState (ButtonBehaviour.State.Play);
        //PhotonNetwork.JoinRandomRoom();
        roomsCanvas.GetComponent<Canvas> ().enabled = true;
    }

    public void OnCreateRoomButtonClicked() {
        Debug.Log("Trying to create a new room");
        RoomOptions roomOps = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom(roomNameInputField.textComponent.text, roomOps);
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

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancelbutton was clicked");
        mainCanvasButtonBehaviour.ChangeButtonState (ButtonBehaviour.State.Play);
        PhotonNetwork.LeaveRoom();
    }
    
    // This is probably redundant now. (it still exists in PhotonLobbyVR)
    public void OnVrPlayerReadyToJoinGame ()
    {
        VrPlayerSeesJoinButton = true;
        mainCanvasButtonBehaviour.ChangeButtonState (ButtonBehaviour.State.Play);
    }
}
