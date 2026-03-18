using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;
    public bool isMoving = false;

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
        isMoving = moveDirection.magnitude > 0;



    }
}
