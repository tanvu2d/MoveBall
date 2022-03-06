using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]
    public int x, y;
    [HideInInspector]
    public TypeBall typeBall;

    public void SetPos(int _x, int _y)
    {
        transform.position = new Vector3(_x, transform.position.y, _y);
        x = (int)(transform.position.x);
        y = (int)(transform.position.z);
    }


    IEnumerator IEDoScaleXY(float from, float to, float _time)
    {
        float timeCount = Time.deltaTime;
        int cout = 0;
        while (timeCount < _time)
        {
            Vector3 scale = transform.localScale;
            scale.y = from;
            scale.x = from;
            transform.localScale = scale;
            from = (timeCount / _time) / 2f;
            timeCount += Time.deltaTime;
            cout++;
            yield return null;
        }
    }

    public void DoScaleXY(float from, float to, float _time)
    {
        StartCoroutine(IEDoScaleXY(from, to, _time));
    }

}

public enum TypeBall
{
    blue,
    red,
    black,
    yellow,
    ghost,
}

