
using UnityEngine;

public class MazeLoader : MonoBehaviour
{
    public int mazeRows, mazeColumns;
    public GameObject wall;
    public GameObject floor;
    public GameObject wallWithLight;
    public float size = 2f;
    public GameObject mazeParent;
    public GameObject floorParent;

    private MazeCell[,] mazeCells;

    public int randomDirection;
    public int randomRemovableWallNumber;
    public bool exitRemoved = false;
    public GameObject destroyableWall;

    public bool isMazeGenerationComplete = false;
    
    public GameObject winCollider;

    //public static List<int> KillKeys = new List<int>();
    //public static List<int> DestroyKeys = new List<int>();
    //public List<int> KillKeys2 = new List<int>();
    //public List<int> DestroyKeys2 = new List<int>();

    //Is the player ar or vr?
    //ar = maze is randomized
    //vr = maze is created with ar KillKeys and DestroyKeys values

    //public static bool arPlayer = true;

    // Use this for initialization
    void Awake()
    {
        randomDirection = Random.Range(1, 5);
        randomRemovableWallNumber = Random.Range(0, Mathf.Min(mazeRows, mazeColumns));

        InitializeMaze();

        MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
        ma.CreateMaze();

    }

    private void Start()
    {
        /*
        KillKeys = HuntAndKillMazeAlgorithm.KillKeys;
        DestroyKeys = HuntAndKillMazeAlgorithm.DestroyKeys;

        KillKeys2 = KillKeys;
        DestroyKeys2 = DestroyKeys;
        */
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InitializeMaze()
    {

        mazeCells = new MazeCell[mazeRows, mazeColumns];

        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();

                mazeCells[r, c].floor = Instantiate(floor, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
                mazeCells[r, c].floor.name = "Floor " + r + "," + c;
                mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                mazeCells[r, c].floor.transform.parent = floorParent.transform;

                if (c == 0)
                {
                    mazeCells[r, c].westWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;
                    mazeCells[r, c].westWall.transform.parent = mazeParent.transform;
                }

                if (c % 3 == 0)
                {
                    mazeCells[r, c].eastWall = Instantiate(wallWithLight, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
                }

                else
                {
                    mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
                }

                mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;
                mazeCells[r, c].eastWall.transform.parent = mazeParent.transform;

                if (r == 0)
                {
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r * size) - (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                    mazeCells[r, c].northWall.transform.parent = mazeParent.transform;
                }

                if(c % 3 == 0)
                {
                    mazeCells[r, c].southWall = Instantiate(wallWithLight, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                }

                else
                {
                    mazeCells[r, c].southWall = Instantiate(wall, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                }
                
                mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
                mazeCells[r, c].southWall.transform.parent = mazeParent.transform;
            }
        }

        if (MazeValues.isArPlayer)
        {
            //This is if we want to randomize exit wall
            /*
            if (randomDirection == 1)
            {
                //west
                destroyableWall = GameObject.Find("/Maze/West Wall " + randomRemovableWallNumber.ToString() + "," + 0.ToString());
            }
            else if (randomDirection == 2)
            {
                //east
                destroyableWall = GameObject.Find("/Maze/East Wall " + randomRemovableWallNumber.ToString() + "," + (mazeColumns - 1).ToString());
                //Destroy(destroyableWall);
            }
            else if (randomDirection == 3)
            {
                //north
                destroyableWall = GameObject.Find("/Maze/North Wall " + 0.ToString() + "," + randomRemovableWallNumber.ToString());
                //Destroy(destroyableWall);
            }
            else if (randomDirection == 4)
            {
                //south
                destroyableWall = GameObject.Find("/Maze/South Wall " + (mazeRows - 1).ToString() + "," + randomRemovableWallNumber.ToString());
                //Destroy(destroyableWall);
            }

            MazeValues.destroyableWall = destroyableWall.name;
            */

            //This is for always the same wall
            Instantiate(winCollider, GameObject.Find("/Maze/South Wall 11,14").gameObject.transform.position, GameObject.Find("/Maze/South Wall 11,14").gameObject.transform.rotation);
            Destroy(GameObject.Find("/Maze/South Wall 11,14"));
        }

        else if (MazeValues.isArPlayer == false)
        {
            destroyableWall = GameObject.Find(destroyableWall.name);
        }

        

        if (destroyableWall)
        {
            Destroy(destroyableWall);
        }

        else
        {
            Debug.Log("ERROR: no exit information");
        }

        isMazeGenerationComplete = true;
    }
}