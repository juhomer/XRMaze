using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Cast ray to demonstrate board game in real world
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceTheBoard : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Instantiate this prefab on a plane at the ray")]
    GameObject board;
    Transform boardTransform;
    [SerializeField] GameObject VRPlayerPrefab;

    ARRaycastManager RaycastManager;
    ARPlaneManager PlaneManager;
    ARSessionOrigin SessionOrigin;
    //ARPointCloudManager PointCloudManager;


    Vector2 centerScreenPos;
    static List<ARRaycastHit> listHits = new List<ARRaycastHit>();
    [SerializeField] Button PlaceHereButton;
    Pose hitPose;
    bool boardIsPlaced = false;

    // set starting position for VR player references
    [SerializeField] Button SetVRPositionButton;
    [SerializeField] Button TransferOwnershipButton;
    bool positionVRPlayerIsSet = false;

    [SerializeField] GameObject MazeParent;
    [SerializeField] GameObject FloorParent;
    [SerializeField] GameObject MazeManager;

    //Timer delayRespawnNewBoardTimer;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    GameObject spawnedObject { get; set; }

    GameObject vrCharacter { get; set; }

    private void Awake()
    {
        RaycastManager = GetComponent<ARRaycastManager>();
        PlaneManager = GetComponent<ARPlaneManager>();
        SessionOrigin = GetComponent<ARSessionOrigin>();
        //PointCloudManager = GetComponent<ARPointCloudManager>();

        SetVRPositionButton.gameObject.SetActive(false);
        TransferOwnershipButton.gameObject.SetActive(false);

        MazeManager.SetActive(false);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        centerScreenPos = new Vector2(Screen.width / 2, Screen.height / 2);

        //// add listener for restart game event
        //EventManager.AddListener(EventName.RestartGameEvent, HandleRestartGameEvent);

        //// add listener for game over event
        //EventManager.AddListener(EventName.GameOverEvent, HandleGameOverEvent);

        //// create timer
        //delayRespawnNewBoardTimer = gameObject.AddComponent<Timer>();
        //delayRespawnNewBoardTimer.Duration = 0.5f;
        //delayRespawnNewBoardTimer.AddTimerFinishedEventListener(HandleDelayRespawnNewBoardTimerFinishedEvent);

        PlaceHereButton.onClick.AddListener(PositionSelected);

        // set starting position for VR player support
        SetVRPositionButton.onClick.AddListener(SetVRPlayerPosition);
        TransferOwnershipButton.onClick.AddListener(TransferOwnershipToVRPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("VR " + VRPlayerPrefab.transform.position);

        if (!boardIsPlaced)
        {
            if (RaycastManager.Raycast(centerScreenPos, listHits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                hitPose = listHits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(board);
                    SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hitPose.position, hitPose.rotation);
                }
                else
                {
                    SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hitPose.position);
                }
            }
        }

        if (boardIsPlaced && !positionVRPlayerIsSet)
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log("Ray hit " + hit.collider.name);

                    if (hit.collider.CompareTag("Floor"))
                    {
                        if (vrCharacter == null)
                        {
                            vrCharacter = Instantiate(VRPlayerPrefab);
                        }
                        else
                        {
                            vrCharacter.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            //Debug.Log("Plane " + spawnedObject.transform.position);
                            //Debug.Log("VRPlayerPrefab " + vrCharacter.transform.position);
                        }

                    }
                }
            }
        }
    }

    private void PositionSelected()
    {
        PlaceBoard();

        // diactivate existings trackable
        foreach (ARPlane plane in PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        //foreach (ARPointCloud pointCloud in PointCloudManager.trackables)
        //{
        //    pointCloud.gameObject.SetActive(false);
        //}

        // dissable plane and point cloud detections
        //PointCloudManager.enabled = !PointCloudManager.enabled;
        PlaneManager.enabled = !PlaneManager.enabled;

        PlaceHereButton.gameObject.SetActive(false);

        SetVRPositionButton.gameObject.SetActive(true);        

        MazeParent.transform.position = spawnedObject.transform.position;
        MazeParent.transform.rotation = spawnedObject.transform.rotation;

        FloorParent.transform.position = spawnedObject.transform.position;
        FloorParent.transform.rotation = spawnedObject.transform.rotation;

        MazeManager.SetActive(true);
        
        Destroy(spawnedObject.gameObject);

        //Vector3 correctMazePosition = new Vector3(hitPose.position.x - 35, hitPose.position.y, hitPose.position.z - 50);

        //FloorParent.transform.position = correctMazePosition;
        //MazeParent.transform.position = correctMazePosition;

        Debug.Log("Ready to set postion for VR player");
    }

    void PlaceBoard()
    {
        Debug.Log("Board placed!");
        boardIsPlaced = true;
        //boardTransform = GameObject.FindGameObjectWithTag("PlayArea").gameObject.transform;
    }

    void SetVRPlayerPosition()
    {
        positionVRPlayerIsSet = true;

        SetVRPositionButton.gameObject.SetActive(false);

        Transform VRPlayerTransform = vrCharacter.transform;

        Destroy(vrCharacter.gameObject);

        vrCharacter = PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "VRPlayerAvatar"), VRPlayerTransform.position, VRPlayerTransform.rotation, 0);
        
        Debug.Log("VR player postion is set!");

        TransferOwnershipButton.gameObject.SetActive(true);
    }

    void TransferOwnershipToVRPlayer()
    {
        Player[] otherPlayersList = PhotonNetwork.PlayerListOthers;
        //foreach (Player player in otherPlayersList)
        //{
        //    Debug.Log("One player: " + player.UserId);
        //}

        vrCharacter.GetComponent<PhotonView>().TransferOwnership(otherPlayersList[0]);

        TransferOwnershipButton.gameObject.SetActive(false);

        Debug.Log("Ownership transfer!");
    }

    ///// <summary>
    ///// Handle restart game event
    ///// </summary>
    ///// <param name="unused">unused</param>
    //void HandleRestartGameEvent (int unused)
    //{
    //    Time.timeScale = 1;
    //    delayRespawnNewBoardTimer.Run();
    //}

    //void HandleDelayRespawnNewBoardTimerFinishedEvent()
    //{
    //    GameObject newBoard = Instantiate(board);
    //    newBoard.transform.position = boardTransform.position;
    //    newBoard.transform.rotation = boardTransform.rotation;

    //    PlaceBoard();
    //}

    ///// <summary>
    ///// Handle game over event
    ///// </summary>
    ///// <param name="unused">unused</param>
    //void HandleGameOverEvent(int unused)
    //{
    //    Destroy(GameObject.FindGameObjectWithTag("PlayArea").gameObject);
    //}
}
