using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPainter : MonoBehaviour
{
    [Header("原地图")]
    public GameObject originalMap;
    [SerializeField] private PlayerMovement mapBounds;
    
    [Header("目标地图")]
    public SpriteRenderer targetMapRenderer;
    [SerializeField] private Texture2D targetTexture;
    [SerializeField] private Texture2D originalTexture; // 保存原始纹理用于重置
    
    [Header("画笔设置")]
    public Color paintColor = Color.red;
    public int brushSize = 5;
    public float brushHardness = 0.8f;
    
    [Header("玩家")]
    public GameObject player;
    public float paintInterval = 0.1f;
    [SerializeField] private float lastPaintTime;
    
    void Start()
    {
        mapBounds = player.GetComponent<PlayerMovement>();
        
        if (targetMapRenderer != null)
        {
            // 获取要修改的纹理
            targetTexture = targetMapRenderer.sprite.texture;
            
            if (!targetTexture.isReadable)
            {
                Debug.LogError("纹理不可读写！请在导入设置中开启 Read/Write Enabled");
                return;
            }
            
            targetTexture = new Texture2D(
            originalTexture.width, 
            originalTexture.height, 
            TextureFormat.RGBA32,
            false
            );
        }
    }
    
    void Update()
    {
        if (Time.time - lastPaintTime >= paintInterval)
        {
            PaintAtPlayerPosition();
            lastPaintTime = Time.time;
        }
    }
    
    void PaintAtPlayerPosition()
    {
        if (mapBounds == null || targetTexture == null) return;
        
        Vector2 relativePos = mapBounds.GetRelativePos();
        
        int pixelX = Mathf.RoundToInt(relativePos.x * targetTexture.width);
        int pixelY = Mathf.RoundToInt(relativePos.y * targetTexture.height);
        
        pixelX = Mathf.Clamp(pixelX, 0, targetTexture.width - 1);
        pixelY = Mathf.Clamp(pixelY, 0, targetTexture.height - 1);
        
        DrawCir(targetTexture, pixelX, pixelY, brushSize, paintColor, brushHardness);
        
        targetTexture.Apply();
        
    }
    
    void DrawCir(Texture2D tex, int centerX, int centerY, int radius, Color color, float hardness)
    {
        int minX = Mathf.Max(0, centerX - radius);
        int maxX = Mathf.Min(tex.width - 1, centerX + radius);
        int minY = Mathf.Max(0, centerY - radius);
        int maxY = Mathf.Min(tex.height - 1, centerY + radius);
        
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                
                if (dist <= radius)
                {
                    float alpha = 1f;
                    if (hardness < 1f)
                    {
                        float edgeDist = dist / radius;
                        alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01((edgeDist - hardness) / (1f - hardness)));
                    }
                    
                    Color currentColor = tex.GetPixel(x, y);
                    
                    Color newColor = Color.Lerp(currentColor, color, alpha);
                    
                    tex.SetPixel(x, y, newColor);
                }
            }
        }
    }
    
    // 重置纹理
    public void ResetTexture()
    {
        if (originalTexture != null && targetTexture != null)
        {
            targetTexture.SetPixels(originalTexture.GetPixels());
            targetTexture.Apply();
        }
    }
        public void SaveTexture(string filename)
    {
        byte[] bytes = targetTexture.EncodeToPNG();
        string path = Application.dataPath + "/" + filename + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log($"纹理已保存到: {path}");
    }
}
