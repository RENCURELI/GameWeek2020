using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE : WILL HAVE TO ADD GRID POINT INDEXES TO SIMULATE NUMERAL GRID SPACES


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

    #endregion

    private void Awake()
    {
        gridArray = new GameObject[columns , rows];

        if (gridPrefab)
            GenerateGrid();
        else
            print("Prefab missing");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Functions

    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject instance = Instantiate(gridPrefab, new Vector3(generationStart.x + scale * i, generationStart.y, generationStart.z + scale * j), Quaternion.identity);
                instance.transform.SetParent(gameObject.transform);
                instance.GetComponent<GridStat>().x = i;
                instance.GetComponent<GridStat>().y = j;

                gridArray[i, j] = instance;
            }
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
                if (instance.GetComponent<GridStat>().visited == step-1)
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

    #endregion
}
