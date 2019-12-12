using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.EventSystems;

public class RoomListing : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Text _text;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo) {
        RoomInfo = roomInfo;
        _text.text = " " + roomInfo.Name;
    }

    public void OnRoomClick() {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

    public void OnPointerClick (PointerEventData ped)
    {
        PhotonNetwork.JoinRoom (RoomInfo.Name);
    }
}
