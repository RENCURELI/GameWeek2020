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

    #endregion

    private void Awake()
    {
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
            }
        }
    }


    #endregion
}
