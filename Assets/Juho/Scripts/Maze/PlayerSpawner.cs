using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Vector3 spawnLocation;
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer(spawnLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(Vector3 location)
    {
        Instantiate(player, location, Quaternion.identity);
    }
}
