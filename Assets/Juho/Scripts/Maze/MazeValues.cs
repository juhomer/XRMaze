using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MazeValues : MonoBehaviour/*, IPunObservable*/
{
    //This script has all the values that need to be networked

    //is player AR player
    public static bool isArPlayer = true;

    //this is needed for the Maze creation. If isArPlayer = false then we generate it, otherwise we use it
    public static List<int> KillKeys = new List<int> ();
    public static List<int> DestroyKeys = new List<int> ();

    //This is needed for destroying the exit
    public static string destroyableWall;

    //This is the location where we want to spawn the player
    public static Vector3 spawnLocation;


    //These values are just for testing purposes
    public bool isArPlayer2;
    public List<int> KillKeys2 = new List<int> ();
    public List<int> DestroyKeys2 = new List<int> ();
    public string destroyableWall2;

    private PhotonView photonView;

    private void Start()
    {
        //#if UNITY_IOS || UNITY_ANDROID
        //        isArPlayer = true;
        //#elif UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        //        isArPlayer = false;
        //#endif
        photonView = GameObject.Find("RoomController").GetComponent<PhotonView>();

        KillKeys = HuntAndKillMazeAlgorithm.KillKeys;
        DestroyKeys = HuntAndKillMazeAlgorithm.DestroyKeys;

        //isArPlayer2 = isArPlayer;
        //KillKeys2 = KillKeys;
        //DestroyKeys2 = DestroyKeys;
        //destroyableWall2 = destroyableWall;


        if (isArPlayer)
        {
            OnReadyToGenerateMaze();
            photonView.RPC(nameof(UnmasterOthers), RpcTarget.OthersBuffered);
        }
    }

    [PunRPC]
    private void SendMazeData<T> (T cacca)
    {
        
        // GENERATE EACH INDIVIDUAL MAZE VALUE HERE!
        // FOR EXAMPLE CALL THIS FUNCTION SEPARATELY FOR KILL KEYS, AND DESTROY KEYS (SEPARATE FUNCTION NEEDED FOR SENDING DESTROYABLE WALL SINCE ITS STRING
        // AND JUHO DEAR, IF THIS GENERIC FUNCTION DOES NOT WORK (I'M NOT SURE ABOUT HOW PHOTON HANDLES GENERICS)
        // CREATE SEPARATE FUNCTIONS WHERE THERE ARE PARAMETERS FOR LIST AND STRING. RIGHT NOW I HOPE THAT THIS FUNCTIONS HANDLES BOTH <3
    }

    [PunRPC]
    private void UnmasterOthers()
    {
        print("NOT MASTER");
        isArPlayer = false;
    }

    [PunRPC]
    public void RefreshKey(string newKey)
    {
        print("KEY SENT");
        ProceduralNumberGenerator.key = newKey;
    }




    public void OnReadyToGenerateMaze ()
    {
        if (isArPlayer)
        {
            if (!photonView)
                return;
            //THIS NEEDS TO BE CALLED ON GAMEOBJECT WHERE PHOTON VIEW COMPONENT IS ADDED(i think :D)

            photonView.RPC(nameof(RefreshKey), RpcTarget.AllBuffered, ProceduralNumberGenerator.key);
            photonView.RPC (nameof (SendMazeData), RpcTarget.OthersBuffered, destroyableWall);
            return;
        }
        //IF CODE GETS THIS FAR IT MEANS WE ARE VR PLAYER. WHEN AR PLAYER CALLS THIS SCRIPT IT IS AUTOMATICALLY UPDATED ON OTHER(S)
    }

    //THIS MIGHT BE REDUNDANT
    /*public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext (isArPlayer);
            stream.SendNext (KillKeys);
            stream.SendNext (DestroyKeys);
            stream.SendNext (destroyableWall);
            stream.SendNext (spawnLocation);
        }
        else
        {
            isArPlayer = (bool) stream.ReceiveNext ();
            KillKeys = (List<int>) stream.ReceiveNext ();
            DestroyKeys = (List<int>) stream.ReceiveNext ();
            destroyableWall = (string) stream.ReceiveNext ();
            spawnLocation = (Vector3) stream.ReceiveNext ();
        }
    }*/
}