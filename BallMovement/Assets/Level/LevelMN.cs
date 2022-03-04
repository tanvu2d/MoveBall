using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMN : Singleton<LevelMN>
{
    [SerializeField] Area preArea;
    public int weigh = 10;
    public int heigh = 10;
    [SerializeField] Transform holderArea;

    private Area[,] arryArea;

    private const int MOVE_STRAIGHT_COST = 10;
    [SerializeField] BallController preBall;
    [SerializeField] Transform transBallHoder;

    protected override void Awake()
    {
        base.Awake();
        CreateMap();
    }
    void CreateMap()
    {
        arryArea = new Area[weigh, heigh];
        for (int x = 0; x < weigh; x++)
        {
            for (int y = 0; y < heigh; y++)
            {
                Area item = Instantiate(preArea, holderArea);
                item.transform.position = new Vector3(x, item.transform.position.y, y);
                item.x = x;
                item.y = y;
                arryArea[x, y] = item;


            }
        }

        CreateBallLV();
    }

    public void CreateBall(int posX, int posZ)
    {

        BallController itemBall = Instantiate(preBall, transBallHoder);
        itemBall.transform.position = new Vector3((float)posX, 0.3f, (float)posZ );
        int typeBall = (int)Random.Range(0, 4);
        itemBall.SetTypeBall(typeBall);
        arryArea[posX, posZ].currentBall = itemBall;

    }

    void CreateBallLV()
    {
        for (int i = 0; i < weigh; i = i + 3)
        {
            for (int j = 0; j < heigh; j = j + 3)
            {
                CreateBall(i, j);
            }
        }


        CreateBall(1, 7);
        CreateBall(2, 8);
        CreateBall(1, 6);
        CreateBall(2, 7);
        CreateBall(3, 8);
    }
    Area GetItemArr(Vector3 pointWorld, out int x, out int y)

    {
        x = (int)(pointWorld.x);
        y = (int)(pointWorld.z);
        return arryArea[x, y];
    }

    public List<Vector3> FindPath(Vector3 startPointEv, Vector3 EndPointEv)
    {
        Area startArea = GetItemArr(startPointEv, out int startX, out int startZ);
        Area endArea = GetItemArr(EndPointEv, out int endX, out int endZ);

        List<Area> path = FindPath(startX, startZ, endX, endZ);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Area pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, 0.3f, pathNode.y));
            }
            return vectorPath;
        }
    }


    public List<Area> FindPath(int startX, int startY, int endX, int endY)
    {
        Area startNode = arryArea[startX, startY];
        Area endNode = arryArea[endX, endY];

        if (startNode == null || endNode == null)
        {
            return null;
        }

        List<Area> openList = new List<Area> { startNode };
        List<Area> closedList = new List<Area>();


        for (int x = 0; x < weigh; x++)
        {
            for (int y = 0; y < heigh; y++)
            {
                Area pathNode = arryArea[x, y];
                pathNode.gCost = 999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            Area currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Area neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (neighbourNode.currentBall != null)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }

            }
        }
        closedList.Clear();
        return closedList;
    }

    int CalculateDistanceCost(Area a, Area b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return MOVE_STRAIGHT_COST * (xDistance + yDistance);

    }

    private Area GetLowestFCostNode(List<Area> pathNodeList)
    {
        Area lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private List<Area> CalculatePath(Area endNode)
    {
        List<Area> path = new List<Area>();
        path.Add(endNode);
        Area currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private List<Area> GetNeighbourList(Area currentNode)
    {
        List<Area> neighbourList = new List<Area>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(arryArea[currentNode.x - 1, currentNode.y]);
            // Left Down
            //     if (currentNode.y - 1 >= 0) neighbourList.Add(arryArea[currentNode.x - 1, currentNode.y - 1]);
            // Left Up
            //     if (currentNode.y + 1 < heigh) neighbourList.Add(arryArea[currentNode.x - 1, currentNode.y + 1]);
        }
        if (currentNode.x + 1 < weigh)
        {
            // Right
            neighbourList.Add(arryArea[currentNode.x + 1, currentNode.y]);
            // Right Down
            //        if (currentNode.y - 1 >= 0) neighbourList.Add(arryArea[currentNode.x + 1, currentNode.y - 1]);
            // Right Up
            //       if (currentNode.y + 1 < heigh) neighbourList.Add(arryArea[currentNode.x + 1, currentNode.y + 1]);
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(arryArea[currentNode.x, currentNode.y - 1]);
        // Up
        if (currentNode.y + 1 < heigh) neighbourList.Add(arryArea[currentNode.x, currentNode.y + 1]);

        return neighbourList;
    }

    public void SetAreaWall(int startX, int startY, int endX, int endY, BallController _ball)
    {
        arryArea[startX, startY].currentBall = null;
        arryArea[endX, endY].currentBall = _ball;
    }

    public bool CheckCanMove(int pointX, int pointY)
    {
        if (arryArea[pointX, pointY].currentBall)
        {
            return false;
        }
        return true;
    }


    public void CheckInstanceBall(int x, int y)
    {
        List<BallController> listLeftRight = new List<BallController>();
        List<BallController> listUpDown = new List<BallController>();
        List<BallController> listDiagonalRight = new List<BallController>();
        List<BallController> listDiagonalLeft = new List<BallController>();
        // Check left Right
        BallController _currentBall = arryArea[x, y].currentBall;
        for (int i = x + 1; i <= i + 4; i++)
        {
            if (i > 9)
                break;

            if (arryArea[i, y].currentBall != null)
            {
                if (arryArea[i, y].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listLeftRight.Add(arryArea[i, y].currentBall);
            }
            else
            {
                break;
            }
        }

        for (int i = x - 1; i >= i - 4; i--)
        {
            if (i < 0)
                break;

            if (arryArea[i, y].currentBall != null)
            {
                if (arryArea[i, y].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listLeftRight.Add(arryArea[i, y].currentBall);
            }
            else
            {
                break;
            }
        }
        // check updown
        for (int i = y + 1; i <= i + 4; i++)
        {
            if (i > 9)
                break;

            if (arryArea[x, i].currentBall != null )
            {
                if (arryArea[x, i].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listUpDown.Add(arryArea[x, i].currentBall);
            }
            else
            {
                break;
            }
        }

        for (int i = y - 1; i >= i - 4; i--)
        {
            if (i < 0)
                break;

            if (arryArea[x, i].currentBall != null )
            {
                if (arryArea[x, i].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listUpDown.Add(arryArea[x, i].currentBall);
            }
            else
            {
                break;
            }
        }
        // check diagonal  right 
        int j = y + 1;
        for (int i = x + 1; i <= i + 4; i++)
        {
            if (i > 9 || j > 9)
                break;

            if (arryArea[i, j].currentBall != null)
            {
                if (arryArea[i, j].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listDiagonalRight.Add(arryArea[i, j].currentBall);
                j++;
            }
            else
            {
                break;
            }
        }
        int j1 = y - 1;
        for (int i = x - 1; i >= i - 4; i--)
        {
            if (i < 0 || j1 < 0)
                break;

            if (arryArea[i, j1].currentBall != null)
            {
                if (arryArea[i, j1].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listDiagonalRight.Add(arryArea[i, j1].currentBall);
                j1--;
            }
            else
            {
                break;
            }
        }
        // check diagonal  left
        int j2 = y - 1;
        for (int i = x + 1; i <= i + 4; i++)
        {
            if (i > 9 || j2 < 0)
                break;

            if (arryArea[i, j2].currentBall != null)
            {
                if (arryArea[i, j2].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listDiagonalLeft.Add(arryArea[i, j2].currentBall);
                j2--;
            }
            else
            {
                break;
            }
        }
        int j3 = y + 1;
        for (int i = x - 1; i >= i - 4; i--)
        {
            if (i < 0 || j3 > 9)
                break;

            if (arryArea[i, j3].currentBall != null )
            {
                if (arryArea[i, j3].currentBall.typeBall != _currentBall.typeBall)
                    break;
                listDiagonalLeft.Add(arryArea[i, j3].currentBall);
                j3++;
            }
            else
            {
                break;
            }
        }
        ///== destroy 
        if (listLeftRight.Count >= 4)
        {
            foreach (BallController i in listLeftRight)
            {
                arryArea[i.x, i.y].currentBall = null;
                Destroy(i.gameObject);
                ScoreMN.Instance.SetMyScore(1);
            }
        }

        if (listUpDown.Count >= 4)
        {
            foreach (BallController i in listUpDown)
            {
                arryArea[i.x, i.y].currentBall = null;
                Destroy(i.gameObject);
                ScoreMN.Instance.SetMyScore(1);
            }
        }

        if (listDiagonalRight.Count >= 4)
        {
            foreach (BallController i in listDiagonalRight)
            {
                arryArea[i.x, i.y].currentBall = null;
                Destroy(i.gameObject);
                ScoreMN.Instance.SetMyScore(1);
            }
        }

        if (listDiagonalLeft.Count >= 4)
        {
            foreach (BallController i in listDiagonalLeft)
            {
                arryArea[i.x, i.y].currentBall = null;
                Destroy(i.gameObject);
                ScoreMN.Instance.SetMyScore(1);
            }
        }

        if (listLeftRight.Count >= 4 || listUpDown.Count >= 4 || listDiagonalRight.Count >= 4 || listDiagonalLeft.Count >= 4)
        {

            BallController ball = arryArea[x, y].currentBall;
            arryArea[x, y].currentBall = null;
            Destroy(ball.gameObject);
            ScoreMN.Instance.SetMyScore(1);
        }
    }

    public void CheckEarnPoint (int x , int y )
    {
        CheckInstanceBall(x, y);
        if (CheckCanSpawnBall())
        {
            Spaw3Ball();
        }
        else
        {
            GameMN.Instance.GameOver();
        }
    }

    public bool CheckCanSpawnBall ()
    {
        List<Area> listArea = new List<Area>();
        for (int i = 0; i < weigh; i++)
        {
            for (int j = 0; j < heigh; j++)
            {
                if (arryArea[i, j].currentBall == null)
                {
                    listArea.Add(arryArea[i, j]);
                }
            }
        }
        if (listArea.Count < 3)
        {
            return false;
        }
        return true;
    }
    public void Spaw3Ball()
    {
        List<Area> listArea = new List<Area>();
        for (int i =0; i < weigh; i++)
        {
            for (int  j =  0; j  <heigh; j ++)
            {
                if (arryArea[i,j].currentBall == null )
                {
                    listArea.Add(arryArea[i, j]);
                }
            }
        }

        int i1 =  0;
        int i2 =  0;
        int i3 =  0;
        while ( i1 == i2  && i2 == i3)
        {
            i1 = (int)Random.Range(0, listArea.Count - 1);
            i2 = (int)Random.Range(0, listArea.Count - 1);
            i3 = (int)Random.Range(0, listArea.Count - 1);
        }

        CreateBall(listArea[i1].x, listArea[i1].y); 
        CreateBall(listArea[i2].x, listArea[i2].y); 
        CreateBall(listArea[i3].x, listArea[i3].y); 
    }

}
            
            
            

        
    

