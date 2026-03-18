using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValController : MonoBehaviour
{

    private float updateInterval = 0.5f;
    private float lastUpdateTime;
    private float smoothSpeed = 5f;


    [Header("玩家脚本")]
    public PlayerMovement playerMovement;


    [Header("水位")]
    public Image waterImage;
    public TMP_Text waterLevelText;
    public float maxWaterLevel = 100f;
    public float waterChangeRate = -0.2f;
    private float currentWaterLevel = 100f;
    private float displayWaterLevel = 100f;


    void Start()
    {
        lastUpdateTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isMoving && Time.time - lastUpdateTime >= updateInterval)
        {
            WaterChange(waterChangeRate);
            lastUpdateTime = Time.time;
        }

        displayWaterLevel = Mathf.MoveTowards(
            displayWaterLevel, 
            currentWaterLevel, 
            smoothSpeed * Time.deltaTime
        );

        waterImage.fillAmount = displayWaterLevel / maxWaterLevel;
    }

    void WaterChange(float delta)
    {
        currentWaterLevel = Mathf.Clamp(currentWaterLevel + delta, 0, maxWaterLevel);
        
        waterLevelText.text = $"{currentWaterLevel:F1}/{maxWaterLevel}";
    }
}
