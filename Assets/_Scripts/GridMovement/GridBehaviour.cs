using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridBehaviour : MonoBehaviour
{

    #region Variables
    /// <summary>
    /// Used to define number of rows on grid
    /// </summary>
    [Header("Number of rows and columns")]
    public int rows = 10;
    /// <summary>
    /// Used to define number of columns on grid
    /// </summary>
    public int columns = 10;

    /// <summary>
    /// Number of units between each point
    /// </summary>
    [Header("Ditance between points")]
    public int scale = 5;

    [Header("Grid Point Prefab")]
    public GameObject gridPrefab;

    /// <summary>
    /// Start point of grid generation
    /// </summary>
    public Vector3 generationStart = new Vector3(0, 0, 0);

    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;

    public int endX = 2;
    public int endY = 2;

    public List<GameObject> path = new List<GameObject>();

    [SerializeField]
    private bool findDistance = false;

    /// <summary>
    /// Is the controlled pawn bound to move?
    /// </summary>
    public bool movePawn = false;

    /// <summary>
    /// Will manage dynamically through turn management and player indexing which player pawn must be moved
    /// </summary>
    [SerializeField]
    private GameObject movedPawn;

    #endregion

    private void Awake()
    {
        gridArray = new GameObject[columns , rows];
        movedPawn = FindObjectOfType<PlayerMovement>().gameObject;

        if (gridPrefab)
            GenerateGrid();
        else
            print("Prefab missing");
    }

    // Update is called once per frame
    void Update()
    {
        if(findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    #region Functions

    void GenerateGrid()
    {
        int tempIndexI = 0;
        int tempIndexJ = 0;
        for (int i = 0; i < columns; i++)
        {
            
            for (int j = 0; j < rows; j++)
            {
                
                GameObject instance = Instantiate(gridPrefab, new Vector3(generationStart.x + scale * i, generationStart.y, generationStart.z + scale * j), Quaternion.identity);
                
                instance.transform.SetParent(gameObject.transform);
                instance.GetComponent<GridStat>().x = i;
                instance.GetComponent<GridStat>().y = j;

                gridArray[i, j] = instance;
                instance.GetComponent<GridStat>().index = tempIndexI + tempIndexJ;
                tempIndexJ++;
            }
            --tempIndexJ;
            tempIndexI++;
        }
    }

    void SetDistance()
    {
        InitialSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        
        for (int step = 1; step < rows*columns; step++)
        {
            foreach (GameObject instance in gridArray)
            {
                if (instance && instance.GetComponent<GridStat>().visited == step-1)
                {
                    TestFourDirections(instance.GetComponent<GridStat>().x, instance.GetComponent<GridStat>().y, step);
                }
            }
        }
    }

    void SetPath()
    {
        int step;
        
        int x = endX;
        int y = endY;
        

        List<GameObject> tempList = new List<GameObject>();

        path.Clear();

        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            print("Can't reach destination");
            return;
        }

        //Check path going from destination to starting point
        for (int i = step; step>-1; step--)
        {
            if (TestDir(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDir(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDir(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDir(x, y, step, 4))
                tempList.Add(gridArray[x - 1, y]);

            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStat>().x;
            y = tempObj.GetComponent<GridStat>().y;
            tempList.Clear();
        }
        FindObjectOfType<PlayerMovement>().nextMoveIndex = path.Count - 1;
        movePawn = true;
    }

    void InitialSetup()
    {
        foreach (GameObject instance in gridArray)
        {
            //No grid points have been visited and are set to -1 (non visited)
            instance.GetComponent<GridStat>().visited = -1;
        }
        //Start point is set to 0 (visited)
        gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    /// <summary>
    /// Tells which direction to follow (1 is Up, 2 is right, 3 is Down, 4 is Left)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="step"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    bool TestDir(int x, int y, int step, int dir)
    {
        
        switch(dir)
        {
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
                {
                    return true;
                }
                else
                    return false;

            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;

            case 3:
                if (y - 1 >= 0 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                {
                    return true;
                }
                else
                    return false;
            case 4:
                if (x - 1 >= 0 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
                {
                    return true;
                }
                else
                    return false;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDir(x,y,-1,1))
        {
            SetVisited(x, y + 1, step);
        }
        if (TestDir(x,y,-1,2))
        {
            SetVisited(x + 1, y, step);
        }
        if (TestDir(x,y,-1,3))
        {
            SetVisited(x, y - 1, step);
        }
        if (TestDir(x,y,-1,4))
        {
            SetVisited(x - 1, y, step);
        }
    }

    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStat>().visited = step;
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNum = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNum = i;
            }
        }

        return list[indexNum];
    }

    #endregion
}
