using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TimeController : MonoBehaviour
{
    [SerializeField] Text txtTimeValue;

    DateTime startTime;

    private void Start()
    {
        startTime = DateTime.Now;
    }

    private void FixedUpdate()
    {
        UpdateTime();
    }
    void UpdateTime()
    {
        TimeSpan time = DateTime.Now - startTime;
        if (time.TotalMinutes > 60)
            txtTimeValue.text = time.ToString(".hh\\:mm\\:ss");
        else
            txtTimeValue.text = time.ToString("mm\\:ss");

    }
}
