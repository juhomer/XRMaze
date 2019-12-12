using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotManager : MonoBehaviour
{
    [SerializeField] GameObject SlingShotPrefab;

    [SerializeField] MazeLoader mazeLoader;

    GameObject[] FloorArray;

    int maxSlingShotOnField = 4;

    Timer respawnNewSlingShotTimer;
    Timer destroySlingShotTimer;

    bool isReadyToRespawnSlingShot = false;

    // Start is called before the first frame update
    void Start()
    {
        // add timer to check whether or not to repawn new slingshot
        respawnNewSlingShotTimer = gameObject.AddComponent<Timer>();
        respawnNewSlingShotTimer.Duration = 10;
        respawnNewSlingShotTimer.AddTimerFinishedEventListener(HandleRespawnNewSlingShotTimerFinishedEvent);

        destroySlingShotTimer = gameObject.AddComponent<Timer>();
        destroySlingShotTimer.Duration = 20;
        destroySlingShotTimer.AddTimerFinishedEventListener(HandleDestroySlingShotTimerFinishedEvent);
    }    

    // Update is called once per frame
    void Update()
    {
        if (mazeLoader.isMazeGenerationComplete && !isReadyToRespawnSlingShot)
        {
            FloorArray = GameObject.FindGameObjectsWithTag("Floor");
            isReadyToRespawnSlingShot = true;

            // start respawn new sling shot timer
            respawnNewSlingShotTimer.Run();
        }
    }

    Vector3 GetSlingShotRespawnPosition()
    {
        int ranFloorNumber = UnityEngine.Random.Range(0, FloorArray.Length);
        Transform floorTransform = FloorArray[ranFloorNumber].transform;
        float cameraPosY = Camera.main.transform.position.y;

        float posX = floorTransform.position.x;
        float posZ = floorTransform.position.z;
        float posY = UnityEngine.Random.Range(cameraPosY / 3, cameraPosY *2/3);

        return new Vector3(posX, posY, posZ);
    }
    
    private void HandleRespawnNewSlingShotTimerFinishedEvent()
    {
        GameObject[] slingShotArray = GameObject.FindGameObjectsWithTag("SlingShot");

        if (slingShotArray.Length < maxSlingShotOnField)
        {
            RespawnSlingShot();
        }

        respawnNewSlingShotTimer.Run();
    }

    void RespawnSlingShot()
    {
        if (isReadyToRespawnSlingShot)
        {
            Instantiate(SlingShotPrefab, GetSlingShotRespawnPosition(), Quaternion.identity);
        }
    }
    private void HandleDestroySlingShotTimerFinishedEvent()
    {
        
    }
}
