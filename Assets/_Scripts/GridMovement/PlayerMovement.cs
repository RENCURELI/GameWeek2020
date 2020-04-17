using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    

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
                
                if (pathList[nextMoveIndex].GetComponent<GridStat>().nodeType == GridStat.NODETYPE.Rest)
                {
                    this.GetComponent<PlayerStats>().hp = 100;
                }
                else if (pathList[nextMoveIndex].GetComponent<GridStat>().nodeType == GridStat.NODETYPE.Ladder)
                {
                    GridStat[] nodes = FindObjectsOfType<GridStat>(); //indexation probably off

                    foreach (GridStat node in nodes)
                    {
                        Debug.Log(node.index);
                    }

                    //Move to next ladder node
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        Debug.Log(currentIndex);
                        if (nodes[i].nodeType == GridStat.NODETYPE.Ladder)
                        {
                            destIndex = nodes[i].index + 1;
                            Debug.Log("USING LADDER" + destIndex);
                            StartPlayerMove();
                            break;
                        }
                    }
                }
                else if (pathList[nextMoveIndex].GetComponent<GridStat>().nodeType == GridStat.NODETYPE.Snake)
                {
                    //Move to next snake node
                    GridStat[] nodes = FindObjectsOfType<GridStat>();

                    foreach (GridStat node in nodes)
                    {
                        Debug.Log(node.index);
                    }
                    //Move to next ladder node
                    for (int i = currentIndex + 1; i < gridManager.gridArray.Length - 1; i++)
                    {
                        Debug.Log(currentIndex);
                        if (nodes[i].nodeType == GridStat.NODETYPE.Snake)
                        {
                            destIndex = nodes[i].index + 1;
                            Debug.Log("USING SNAKE" + destIndex);
                            StartPlayerMove();
                            break;
                        }
                    }
                }
                else
                {
                    FindObjectOfType<CombatManager>().combatStart = true;
                    gridManager.movePawn = false;
                }

                if (!gridManager.movePawn)
                {
                    nextMoveIndex = 0;

                    //gridManager.startX = gridManager.endX;
                    gridManager.startIndex = gridManager.endIndex;
                    gridManager.movePawn = false;
                    return;
                }
            }
                
        }
        
    }

    public void DestDist()
    {
        int dieCast = Random.Range(1, 7);
        destIndex = currentIndex + dieCast;
        StartPlayerMove();
    }

    public void StartPlayerMove()
    {
        gridManager.SetDistance();
        GameObject tempNode;
        foreach (GameObject node in gridManager.gridArray)
        {
            if (node.GetComponent<GridStat>().index == destIndex)
            {
                tempNode = node;
                //gridManager.endX = tempNode.GetComponent<GridStat>().x;
                gridManager.endIndex = tempNode.GetComponent<GridStat>().index;
            }
        }

        gridManager.SetPath();
    }
}
