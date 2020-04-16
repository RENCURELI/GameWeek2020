using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    /// <summary>
    /// Define if point has been visited (-1 = no)
    /// </summary>
    public int visited = -1;
    public int x = 0;
    public int y = 0;

    [SerializeField]
    private GameManager gm;

    /// <summary>
    /// Index used to define grid number
    /// </summary>
    public int index = 0;

    public enum NODETYPE { Standard , Rest , Snake , Ladder }

    public NODETYPE nodeType;
    // Start is called before the first frame update
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        if(nodeType == NODETYPE.Standard)
        {
            GetComponent<Renderer>().material = gm.standardMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
