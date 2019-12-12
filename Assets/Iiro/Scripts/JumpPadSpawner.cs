using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpPadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject jumpPad;
    private MazeLoader mazeLoader;
    private GameObject floor;
    private int row, column;

    [SerializeField] private int spawnCount = 3;
    [SerializeField] private float spawnInterval = 20.0f;

    private bool hasInvokedAlready;

    public bool playerSpawned = false;

    private void Start ()
    {
        mazeLoader = GetComponent<MazeLoader> ();
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
        if (!hasInvokedAlready)
        {
            InvokeRepeating (nameof(SpawnJumpPads), 0, spawnInterval);
            hasInvokedAlready = true;
        }
#endif
    }

    private void SpawnJumpPads()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log ("NO CONNECTION, RETURNING");
            return;
        }

        if (playerSpawned)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                row = Random.Range (0, mazeLoader.mazeRows);
                column = Random.Range (0, mazeLoader.mazeColumns);
                floor = GameObject.Find ("Floor/Floor " + row.ToString () + "," + column.ToString ());

                // Notice that the jump pad prefab name must match whatever is chosen below.
                // PhotonNetwork.InstantiateSceneObject would be better but doesn't work for some reason. (only master can use this?)

                PhotonNetwork.Instantiate ("JumpPad",
                    new Vector3 (floor.transform.position.x, floor.transform.position.y + 0.4f,
                        floor.transform.position.z),
                    Quaternion.identity);

                Debug.Log ("SUCCESFULLY SPAWNED");
            }
        }
    }
}