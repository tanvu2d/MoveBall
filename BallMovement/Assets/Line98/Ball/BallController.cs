using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BallController : Ball
{
    Vector3 endPoint = new Vector3();
    [SerializeField] Renderer renderer;
    [SerializeField] Material materialGhost;
    [SerializeField] GameObject DustTrail;
    protected void Start()
    {
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);
        if (y < LevelMN.Instance.heigh)
            LevelMN.Instance.CheckInstanceBall(x, y);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DustTrail.SetActive(true);
            GameMN.Instance.ClickNewBall(transform.position);
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycashit))
        {
            GameMN.Instance.currentFadeBall.transform.position = new Vector3(raycashit.point.x, 0.3f, raycashit.point.z);
            Vector3 pointEnd = new Vector3(raycashit.point.x, 0.3f, raycashit.point.z) + new Vector3(0.5f, 0f, 0.5f);
            List<Vector3> path = LevelMN.Instance.FindPath(transform.position, pointEnd);
            GameMN.Instance.SetLine(path);
        }
    }

    private void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycashit))
        {
            endPoint = raycashit.point + new Vector3(0.5f, 0, 0.5f);
            if (!GameMN.Instance.isMoving && LevelMN.Instance.CheckCanMove((int)(endPoint.x), (int)(endPoint.z)))
            {
                StartCoroutine(movePath());
            }
        }
    }

    IEnumerator movePath()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        List<Vector3> path = LevelMN.Instance.FindPath(transform.position, endPoint);
        int startX = (int)(transform.position.x);
        int startY = (int)(transform.position.z);
        int i = 0;
        GameMN.Instance.isMoving = true;
        while (i < path.Count)
        {
            transform.position = path[i];
            List<Vector3> pathCurrent = new List<Vector3>();
            for (int j = i; j < path.Count; j++)
            {
                pathCurrent.Add(path[j]);
                GameMN.Instance.SetLine(pathCurrent);
            }
            i++;
            yield return wait;
        }
        GameMN.Instance.currentFadeBall.SetActive(false);
        GameMN.Instance.isMoving = false;
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);
        LevelMN.Instance.SetAreaWall(startX, startY, x, y, this);
        LevelMN.Instance.CheckOverLapBall(x, y);
        if (path.Count > 0)
        {
            LevelMN.Instance.CheckEarnPoint(x, y);
        }
    }

    public void SetTypeBall(int amount)
    {
        switch (amount)
        {
            case 0:
                typeBall = TypeBall.blue;
                renderer.material.color = Color.blue;

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

            case 4:
                typeBall = TypeBall.ghost;
                renderer.material = materialGhost;
                break;
        }
    }
}

