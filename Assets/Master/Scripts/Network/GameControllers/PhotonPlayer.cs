using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        int spawnPicker = 0;

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            spawnPicker = GameSetup.GS.spawnPoints.Length - 1;
        }
        else
        {
            spawnPicker = GameSetup.GS.spawnPoints.Length - 1;
        }

        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
        }
    }
}
