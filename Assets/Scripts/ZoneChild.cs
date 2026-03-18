using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ZoneChild : MonoBehaviour
{
    [Header("调试")]
    public bool showDebugLog = true;

    public ZoneType zoneType;
    private ZoneDetect parentZone;
    
    void Start()
    {
        parentZone = GetComponentInParent<ZoneDetect>();
        
        if (parentZone == null)
        {
            Debug.LogError($"ZoneTrigger on {gameObject.name}: 父物体上没有 ZoneDetect 组件！");
        }
        
        // 确保有 Collider 并设置为触发器
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError($"ZoneTrigger on {gameObject.name}: 没有 Collider2D 组件！");
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && parentZone != null)
        {
            parentZone.OnChildTriggerEnter(gameObject, other.gameObject, zoneType);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && parentZone != null)
        {
            parentZone.OnChildTriggerExit(gameObject, other.gameObject, zoneType);
        }
    }
}
