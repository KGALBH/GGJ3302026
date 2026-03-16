using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private SpriteRenderer map;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        rb.velocity = moveDirection * speed;   


    }

    /// <summary>
    /// Gets the player's position relative to the map's transform, normalized to a unit vector.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRelativePos()
    {
        float x = (transform.position.x - map.transform.position.x) / map.size.x + 0.5f;
        float y = (transform.position.y - map.transform.position.y) / map.size.y + 0.5f;
        return new Vector2(Mathf.Clamp01(x), Mathf.Clamp01(y));
    }
}
