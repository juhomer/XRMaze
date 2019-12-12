using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public Transform spawnLocation;

    public Vector3 westEndPoint;
    public Vector3 eastEndPoint;
    public Vector3 northEndPoint;
    public Vector3 southEndPoint;

    public List<GameObject> floorLocations = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            floorLocations.Add(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
