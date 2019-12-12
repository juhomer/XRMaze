using Photon.Pun;
using UnityEngine;


namespace Iiro.Scripts
{
    public class RoomAvailabilityChecker : MonoBehaviour
    {
        [SerializeField] private PhotonLobbyVR photonLobby;

        private bool shouldStopInvokes;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating (nameof(CheckRoomAvailability), 0f, 2f); //create a boolean to stop this at some point
        }

        private void CheckRoomAvailability ()
        {
            if (PhotonNetwork.CountOfRooms > 0)
            {
                photonLobby.OnVrPlayerReadyToJoinGame ();
            }
        }

        private void Update ()
        {
            if (shouldStopInvokes)
            {
                CancelInvoke(nameof(photonLobby.OnVrPlayerReadyToJoinGame));
            }
        }
    }
}