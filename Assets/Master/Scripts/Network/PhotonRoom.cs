using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{

    public static PhotonRoom room;
    private PhotonView PV;

    public int currentScene;
    public int multiplayerScene;
    public int arScene;

    public Vector3 spawnPosition;

    private void Awake() {

        //setting up the singleton
        if (PhotonRoom.room == null) {
            PhotonRoom.room = this;
        }
        else {
            if (PhotonRoom.room != this) {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable() {

        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += onSceneFinishedLoading;
    }

    public override void OnDisable() {

        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= onSceneFinishedLoading;
    }

    // Start is called before the first frame update
    void Start() {

        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnJoinedRoom() {

        base.OnJoinedRoom();
        Debug.Log("We are now in a room!");
        StartGame();
    }

    void StartGame() {

        if (!PhotonNetwork.IsMasterClient) {

            Debug.Log("Platform: " + Application.platform.ToString());
           
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                PhotonNetwork.LoadLevel(arScene);
            }
            else
            {
                PhotonNetwork.LoadLevel(multiplayerScene);
            }

            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            PhotonNetwork.LoadLevel(arScene);
        }
        else
        {
            PhotonNetwork.LoadLevel(multiplayerScene);
        }

        Debug.Log("Platform: " + Application.platform.ToString());
    }

    void onSceneFinishedLoading(Scene scene, LoadSceneMode mode) {

        currentScene = scene.buildIndex;
        if(currentScene == multiplayerScene) {
            //CreatePlayer();
        }
    }

    private void CreatePlayer() {

        //Creates player network controller(?) but not player character
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), spawnPosition, Quaternion.identity, 0);
    }
}
