using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Player index used for turn management
    /// </summary>
    public int g_index;

    public int nextMoveIndex = 0;

    public int currentIndex = 0;

    private int destIndex = 0;

    /// <summary>
    /// Player movement speed (Visual speed during pawn movement)
    /// </summary>
    public int speed;

    GridBehaviour gridManager;

    List<GameObject> pathList = new List<GameObject>();
    
    void Awake()
    {
        gridManager = FindObjectOfType<GridBehaviour>();
        speed = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (gridManager.movePawn == true && nextMoveIndex >= 0)
        {
            pathList = new List<GameObject>(gridManager.path);
            transform.position = Vector3.MoveTowards(transform.position, pathList[nextMoveIndex].transform.position, Time.deltaTime * speed);

            if (transform.position == pathList[nextMoveIndex].transform.position)
                nextMoveIndex = (nextMoveIndex - 1);
            else if (nextMoveIndex <= 0)
            {
                currentIndex = pathList[nextMoveIndex].GetComponent<GridStat>().index;
                nextMoveIndex = 0;
                
                gridManager.startX = gridManager.endX;
                gridManager.startY = gridManager.endY;
                gridManager.movePawn = false;
                return;
            }
                
        }
        
    }

    public void DestDist()
    {
        int dieCast = Random.Range(1, 7);
        Debug.Log("CAST VALUE : " + dieCast);
        //nextMoveIndex = currentIndex + dieCast;
        destIndex = currentIndex + dieCast;
        Debug.Log("DESTINATION INDEX : " + destIndex);
        gridManager.SetDistance();
        GameObject tempNode;
        foreach (GameObject node in gridManager.gridArray)
        {
            if (node.GetComponent<GridStat>().index == destIndex)
            {
                tempNode = node;
                gridManager.endX = tempNode.GetComponent<GridStat>().x;
                gridManager.endY = tempNode.GetComponent<GridStat>().y;
            }
        }

        gridManager.SetPath();
    }
}
