using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BallController : MonoBehaviour
{

    bool isMoving = false;
    Vector3 endPoint = new Vector3();
    public int x, y;

    public TypeBall typeBall;

    [SerializeField] Renderer renderer;


    private void Start()
    {
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);
        LevelMN.Instance.CheckInstanceBall(x, y);
    }

    private void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray , out RaycastHit raycashit))
        {
             endPoint = raycashit.point + new Vector3(0.5f, 0, 0.5f);
            if (!isMoving && LevelMN.Instance.CheckCanMove((int)(endPoint.x), (int)(endPoint.z)))
            {
                StartCoroutine(movePath()); 
            } 
            
        }
    }

    IEnumerator movePath ()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        List<Vector3> path = LevelMN.Instance.FindPath(transform.position, endPoint);
        int startX = (int)(transform.position.x);
        int startY = (int)(transform.position.z);
        int i = 0;
        isMoving = true;
        while (i < path.Count)
        {
            transform.position = path[i];
            i++;
            yield return wait;
        }
        isMoving = false;
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);
        LevelMN.Instance.SetAreaWall(startX, startY, x, y, this);
        LevelMN.Instance.CheckEarnPoint(x, y);

    }

    public void SetTypeBall (int amount)
    {
        switch (amount)
        {
            case 0:
                typeBall = TypeBall.white;
                renderer.material.color = Color.white;

                break;
            case 1:
                typeBall = TypeBall.red;
                renderer.material.color = Color.red;
                break;

            case 2:
                typeBall = TypeBall.black;
                renderer.material.color = Color.black;
                break;

            case 3:
                typeBall = TypeBall.yellow;
                renderer.material.color = Color.yellow;
                break;

        }

    }
}

public enum TypeBall
{
    white,
    red, 
    black,
    yellow,
}