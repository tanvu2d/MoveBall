using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public BallController currentBall;

    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public Area cameFromNode;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }



}
