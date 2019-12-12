using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{

    public Canvas menuCanvas;
    public Button continueButton;
    public Button exitButton;

    private bool menuActive;

    // Start is called before the first frame update
    void Start()
    {
        //menuCanvas = GameObject.Find("Menu").GetComponent<Canvas>();
        menuCanvas.enabled = false;
        menuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!menuActive) {
                menuCanvas.GetComponent<Canvas>().enabled = true;
                menuActive = true;
            } else {
                menuCanvas.GetComponent<Canvas>().enabled = false;
                menuActive = false;
            }
        }
    }

    public void OnContinueButtonPressed() {
        menuCanvas.GetComponent<Canvas>().enabled = false;
        menuActive = false;
    }

    public void OnExitButtonPressed() {
        //Exit to lobby
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        PhotonNetwork.LoadLevel(0);
    }
}
