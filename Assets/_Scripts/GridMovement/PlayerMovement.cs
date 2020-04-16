using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Player index used for turn management
    /// </summary>
    public int g_index;

    public int nextMoveIndex = 0;

    /// <summary>
    /// Player movement speed (Visual speed during pawn movement)
    /// </summary>
    public int speed;

    //Destination grid square index
    //private int destDistance = 0;

    List<GameObject> pathList = new List<GameObject>();
    
    void Awake()
    {
        speed = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GridBehaviour>().movePawn == true && nextMoveIndex >= 0)
        {
            pathList = new List<GameObject>(FindObjectOfType<GridBehaviour>().path);
            transform.position = Vector3.MoveTowards(transform.position, pathList[nextMoveIndex].transform.position, Time.deltaTime * speed);
            //nextMoveIndex = pathList.Count - 1;

            if (transform.position == pathList[nextMoveIndex].transform.position)
                nextMoveIndex = (nextMoveIndex - 1);
            else if (nextMoveIndex <= 0)
            {
                nextMoveIndex = 0;
                return;
            }
                
        }
        
    }

    public void DestDist()
    {
        int dieCast = Random.Range(1, 7);
        Debug.Log(dieCast);
    }
}
