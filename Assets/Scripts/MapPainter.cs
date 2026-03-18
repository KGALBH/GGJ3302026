using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPainter : MonoBehaviour
{
    [Header("原地图")]
    public SpriteRenderer originalMap;
    
    [Header("目标地图")]
    public SpriteRenderer targetMapRenderer;
    private Texture2D originalTexture;
    private Texture2D targetTexture;
    
    [Header("画笔设置")]
    public Color paintColor = Color.red;
    public int brushSize = 30;
    public float brushHardness = 0.8f;
    
    [Header("玩家")]
    public GameObject player;
    public float paintInterval = 0.01f;
    private float lastPaintTime;
    
    void Start()
    {
        originalTexture = originalMap.sprite.texture;

        if (targetMapRenderer != null)
        {
            targetTexture = targetMapRenderer.sprite.texture;

            targetTexture = new Texture2D(
            originalTexture.width, 
            originalTexture.height, 
            TextureFormat.RGBA32,
            false
            );

            TransparentTexture();
            
            if (!targetTexture.isReadable)
            {
                Debug.LogError("纹理不可读写！请在导入设置中开启 Read/Write Enabled");
                return;
            }
            
            Sprite newSprite = Sprite.Create(
                targetTexture,
                new Rect(0, 0, targetTexture.width, targetTexture.height),
                new Vector2(0.5f, 0.5f),
                100
            );
            targetMapRenderer.sprite = newSprite;

        }
    }
    
    void Update()
    {
        if (Time.time - lastPaintTime >= paintInterval)
        {
            PaintAtPlayerPosition();
            lastPaintTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TransparentTexture();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveTexture("PaintedMap");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CalPixels();
        }
    }

    /// <summary>
    /// Gets the player's position relative to the map's transform, normalized to a unit vector.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRelativePos()
    {
        float x = (player.transform.position.x - originalMap.transform.position.x) / originalMap.size.x + 0.5f;
        float y = (player.transform.position.y - originalMap.transform.position.y) / originalMap.size.y + 0.5f;
        return new Vector2(Mathf.Clamp01(x), Mathf.Clamp01(y));
    }
    

    void PaintAtPlayerPosition()
    {
        if (originalMap == null || targetTexture == null) return;
        
        Vector2 relativePos = GetRelativePos();
        
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
    
    
    public void SaveTexture(string filename)
    {
        byte[] bytes = targetTexture.EncodeToPNG();
        string path = Application.dataPath + "/" + filename + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log($"纹理已保存到: {path}");
    }

    public void TransparentTexture()
    {
        if (targetTexture != null)
        {
            Color[] pixels = new Color[targetTexture.width * targetTexture.height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.clear;
            }
            targetTexture.SetPixels(pixels);
            targetTexture.Apply();
        }
    }

    public float CalPixels()
    {
        if (targetTexture != null)
        {
            int count = 0;
            Color[] pixels = targetTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a > 0)
                {
                    count++;
                }
            }
            return (float)count / (targetTexture.width * targetTexture.height);
        }
        return 0;
    }
}
