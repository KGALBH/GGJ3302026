using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDetect : MonoBehaviour
{

    public ZoneType currentZoneType = ZoneType.normal;


    [Header("调试")]
    public bool showDebugLog = true;

    [Header("玩家脚本")]
    public PlayerMovement playerMovement;

    [Header("数值变化")]
    public ValController valController;
    [SerializeField] private float waterChangeAmount = +2f;

    void Update()
    {
        Changing();
    }
    
    // 当玩家进入任何一个子物体的触发器时调用
    public void OnChildTriggerEnter(GameObject childObject, GameObject player, ZoneType zoneType)
    {
        if (showDebugLog)
        {
            Debug.Log($"玩家进入 {childObject.name}，区域类型：{zoneType}");
        }
        currentZoneType = zoneType;
        EnterZone();
    }

    public void OnChildTriggerExit(GameObject childObject, GameObject player, ZoneType zoneType)
    {
        if (showDebugLog)
        {
            Debug.Log($"玩家离开 {childObject.name}");
        }
        currentZoneType = zoneType;
        ChangingExit();
    }

    public void EnterZone()
    {
        switch (currentZoneType)
        {
            case ZoneType.normal:
                valController.waterChangeRate = -2f;
                break;
            case ZoneType.water:
                valController.waterChangeRate = waterChangeAmount;
                break;
            case ZoneType.sth:
                break;
        }
    }

    public void Changing()
    {
        switch (currentZoneType)
        {
            case ZoneType.normal:
            if (playerMovement.isMoving) valController.UpdateWater();
                break;
            case ZoneType.water:
                valController.UpdateWater();
                break;
            case ZoneType.sth:
                break;
        }
    }

    public void ChangingExit()
    {
        switch (currentZoneType)
        {
            case ZoneType.normal:
                break;
            case ZoneType.water:
                currentZoneType = ZoneType.normal;
                EnterZone();
                break;
            case ZoneType.sth:
                break;
        }
    }
}
