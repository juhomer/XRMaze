using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System.IO;

public class VRAvatarPhoton : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController myCC;

    [SerializeField] GameObject VRControllerPrefab;
    [SerializeField] GameObject vrController;

    int i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView> ();
        myCC = GetComponent<CharacterController> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {

            }

            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && i == 0)
            {
                vrController = Instantiate(VRControllerPrefab, this.transform.position, this.transform.rotation);

                this.transform.SetParent(vrController.transform);

                //Vector3 currentPosition = this.transform.position;

                //this.transform.position = new Vector3(currentPosition.x, currentPosition.y + 1, currentPosition.z);

                GetComponent<MeshRenderer>().enabled = false;

                i++;
            }
        }        
    }
}
