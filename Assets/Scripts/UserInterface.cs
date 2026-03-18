using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UserInterface : MonoBehaviour
{

    [Header("地图覆盖")]
    public MapPainter mapPainter;
    public TMP_Text coverageText;
    private float coverageArea;
    private float updateInterval = 0.5f;
    private float lastUpdateTime;

    // Start is called before the first frame update
    void Start()
    {
        lastUpdateTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - lastUpdateTime < updateInterval)
            return;
        else
        {
            lastUpdateTime = Time.time;
            coverageArea = mapPainter.CalPixels();
            coverageText.text = $"Coverage Area: {(coverageArea * 100f).ToString("F2")}%";
        }

    }
}
