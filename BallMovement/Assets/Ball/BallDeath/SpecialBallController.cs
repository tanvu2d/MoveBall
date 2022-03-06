using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBallController : Ball
{
    private void Start()
    {
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);
    }
    void OnMouseDown()
    {
        LevelMN.Instance.ClickDeathBall(x, y);
    }
}
